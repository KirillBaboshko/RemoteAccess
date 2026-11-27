using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using RemoteAccess.Common.Data;
using RemoteAccess.Contracts.Signaling.Sessions;
using RemoteAccess.Data.DataProviders.Sessions;
using RemoteAccess.Data.DataProviders.Sessions.Model;
using RemoteAccess.Domain.Models.Ecxeptions;
using RemoteAccess.Infrastructure.Settings;

namespace RemoteAccess.Infrastructure.Clients;

public sealed class SignalingSessionHistoryProvider : ISessionHistoryProvider
{
    private readonly SessionHistory.SessionHistoryClient _client;
    private readonly IHttpContextAccessor _http;
    private readonly SignalingApiSettings _settings;

    public SignalingSessionHistoryProvider(SessionHistory.SessionHistoryClient client, IHttpContextAccessor http,
        IOptions<SignalingApiSettings> settings)
    {
        _client = client;
        _http = http;
        _settings = settings.Value;
    }

    public async Task<PagedResult<SessionHistoryModel>> GetHistory(SessionHistoryFilter filter, CancellationToken cancellationToken)
    {
        var request = new GetSessionHistoryRequest
        {
            From = filter.From?.ToTimestamp(),
            To = filter.To?.ToTimestamp(),
            Page = filter.Page,
            PageSize = filter.PageSize
        };
        if (filter.DeviceId is { } deviceId) request.DeviceId = deviceId.ToString();
        if (filter.State is { } state) request.State = state;

        var page = await Call(options => _client.GetListAsync(request, options), cancellationToken);
        return new(page.Items.Select(ToModel).ToList(), page.TotalCount, page.Page, page.PageSize);
    }

    public async Task<SessionHistoryModel?> GetById(Guid sessionId, CancellationToken cancellationToken)
    {
        var request = new GetSessionHistoryByIdRequest { SessionId = sessionId.ToString() };
        try
        {
            var item = await Call(options => _client.GetByIdAsync(request, options), cancellationToken);
            return ToModel(item);
        }
        catch (RpcException error) when (error.StatusCode == StatusCode.NotFound)
        {
            return null;
        }
    }

    private async Task<T> Call<T>(Func<CallOptions, AsyncUnaryCall<T>> send, CancellationToken cancellationToken)
    {
        var authorization = _http.HttpContext?.Request.Headers.Authorization.ToString();
        if (string.IsNullOrEmpty(authorization))
            throw new UnauthorizedAccessException(ValidationErrorMessages.UserIdMissingFromToken);

        var options = new CallOptions(new Metadata { { "authorization", authorization } },
            DateTime.UtcNow.Add(_settings.Timeout), cancellationToken);
        try
        {
            using var call = send(options);
            return await call.ResponseAsync;
        }
        catch (RpcException error) when (error.StatusCode == StatusCode.Cancelled && cancellationToken.IsCancellationRequested)
        {
            throw new OperationCanceledException(error.Message, error, cancellationToken);
        }
        catch (RpcException error) when (error.StatusCode == StatusCode.InvalidArgument)
        {
            throw new InvalidOperationException(error.Status.Detail, error);
        }
        catch (RpcException error) when (error.StatusCode == StatusCode.Unauthenticated)
        {
            throw new UnauthorizedAccessException(ValidationErrorMessages.UserIdMissingFromToken, error);
        }
        catch (RpcException error) when (error.StatusCode is StatusCode.Unavailable or StatusCode.DeadlineExceeded)
        {
            throw new SessionHistoryUnavailableException(error);
        }
    }

    private static SessionHistoryModel ToModel(SessionHistoryItem item) => new(
        Guid.Parse(item.SessionId), Guid.Parse(item.SourceUserId), Guid.Parse(item.TargetUserId),
        Guid.Parse(item.SourceDeviceId), Guid.Parse(item.TargetDeviceId), item.State,
        item.HasEndReason ? item.EndReason : null, item.CreatedAt.ToDateTime(), item.AcceptedAt?.ToDateTime(),
        item.ConnectedAt?.ToDateTime(), item.EndedAt?.ToDateTime(), item.HasDurationSeconds ? item.DurationSeconds : null);
}
