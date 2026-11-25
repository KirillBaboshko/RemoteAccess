using System.Globalization;
using Microsoft.Extensions.Options;
using RemoteAccess.Common.Redis.Abstractions;
using RemoteAccess.Signaling.Domain.Exceptions;
using RemoteAccess.Signaling.Domain.Models.Sessions;
using RemoteAccess.Signaling.Domain.Models.WebRtc;
using RemoteAccess.Signaling.Domain.Repositories;
using RemoteAccess.Signaling.Infrastructure.Redis;
using RemoteAccess.Signaling.Application.Features.Sessions.Models;
using RemoteAccess.Signaling.Infrastructure.Redis.Models;

namespace RemoteAccess.Signaling.Infrastructure.Repositories;

public sealed class RedisSignalingState : ISignalingState
{
    private readonly IRedisClient _redis;
    private readonly SessionOptions _options;
    private readonly LocalConnections _local;

    public RedisSignalingState(IRedisClient redis, IOptions<SessionOptions> options, LocalConnections local)
    {
        _redis = redis;
        _options = options.Value;
        _local = local;
    }

    public async Task ConnectAsync(Guid userId, string connectionId, CancellationToken cancellationToken)
    {
        _local.Add(connectionId);
        try
        {
            await ExecuteAsync(SignalingRedisScripts.Connect, new { userId, connectionId }, cancellationToken);
        }
        catch
        {
            _local.Remove(connectionId);
            throw;
        }
    }

    public async Task RegisterHostAsync(SessionParticipant participant, CancellationToken cancellationToken)
    {
        await ExecuteAsync(SignalingRedisScripts.RegisterHost, participant, cancellationToken);
    }

    public async Task<RemoteSession> CreateSessionAsync(SessionParticipant source, Guid targetDeviceId,
        TimeSpan pendingTimeout, CancellationToken cancellationToken)
    {
        var result = await ExecuteAsync(SignalingRedisScripts.CreateSession, new
        {
            id = Guid.NewGuid(), source, targetDeviceId, pendingTimeout = (long)pendingTimeout.TotalMilliseconds
        }, cancellationToken);
        return result.Session!.ToDomain();
    }

    public async Task<RemoteSession> RespondAsync(Guid sessionId, string connectionId, bool accepted, CancellationToken cancellationToken)
    {
        var result = await ExecuteAsync(SignalingRedisScripts.RespondToRequest, new { id = sessionId, connectionId, accepted }, cancellationToken);
        return result.Session!.ToDomain();
    }

    public async Task<RemoteSession> EndAsync(Guid sessionId, string connectionId, CancellationToken cancellationToken)
    {
        var result = await ExecuteAsync(SignalingRedisScripts.EndSession, new { id = sessionId, connectionId }, cancellationToken);
        return result.Session!.ToDomain();
    }

    public async Task<RemoteSession> PrepareWebRtcMessageAsync(Guid sessionId, Guid userId, string connectionId,
        WebRtcMessageType messageType, CancellationToken cancellationToken)
    {
        var result = await ExecuteAsync(SignalingRedisScripts.PrepareWebRtcMessage,
            new { id = sessionId, userId, connectionId, messageType = (int)messageType }, cancellationToken);
        return result.Session!.ToDomain();
    }

    public async Task<RemoteSession> ReportConnectionStateAsync(Guid sessionId, Guid userId, string connectionId,
        WebRtcConnectionState state, CancellationToken cancellationToken)
    {
        var result = await ExecuteAsync(SignalingRedisScripts.ReportConnectionState,
            new { id = sessionId, userId, connectionId, state = (int)state }, cancellationToken);
        return result.Session!.ToDomain();
    }

    public async Task<IReadOnlyList<RemoteSession>> DisconnectAsync(string connectionId, CancellationToken cancellationToken)
    {
        _local.Remove(connectionId);
        var result = await ExecuteAsync(SignalingRedisScripts.Disconnect, new { connectionId }, cancellationToken);
        return result.Sessions!.Select(session => session.ToDomain()).ToArray();
    }

    public async Task<IReadOnlyList<RemoteSession>> ExpireSessionsAsync(CancellationToken cancellationToken)
    {
        var result = await ExecuteAsync(SignalingRedisScripts.ExpireSessions, new { }, cancellationToken);
        return result.Sessions!.Select(session => session.ToDomain()).ToArray();
    }

    public async Task RenewLeasesAsync(CancellationToken cancellationToken)
    {
        await ExecuteAsync(SignalingRedisScripts.RenewLeases, new { connections = _local.Snapshot() }, cancellationToken);
    }

    private async Task<RedisStateResult> ExecuteAsync(string script, object input, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _redis.ExecuteScriptAsync<RedisStateResult>(script, SignalingRedisKeys.StateKeys,
            [
                _local.NodeId,
                ((long)_options.ConnectionLease.TotalMilliseconds).ToString(CultureInfo.InvariantCulture),
                ((long)_options.SessionLease.TotalMilliseconds).ToString(CultureInfo.InvariantCulture),
                ((long)_options.CleanupGrace.TotalMilliseconds).ToString(CultureInfo.InvariantCulture),
                ((long)_options.NegotiationTimeout.TotalMilliseconds).ToString(CultureInfo.InvariantCulture),
                ((long)_options.ConnectionTimeout.TotalMilliseconds).ToString(CultureInfo.InvariantCulture),
                ((long)_options.RecoveryTimeout.TotalMilliseconds).ToString(CultureInfo.InvariantCulture)
            ], input, cancellationToken);
            result.EnsureSuccess();
            return result;
        }
        catch (RedisClientException)
        {
            throw new SignalingException(ValidationErrorMessages.RedisUnavailable);
        }
    }
}
