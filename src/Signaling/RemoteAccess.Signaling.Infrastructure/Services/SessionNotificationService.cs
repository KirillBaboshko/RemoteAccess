using FluentValidation;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RemoteAccess.Common.Redis.Abstractions;
using RemoteAccess.Signaling.Clients.Signaling;
using RemoteAccess.Signaling.Domain.Models.Sessions;
using RemoteAccess.Signaling.Infrastructure.Redis;
using RemoteAccess.Signaling.Infrastructure.Redis.Models;

namespace RemoteAccess.Signaling.Infrastructure.Services;

public sealed class SessionNotificationService : BackgroundService
{
    private readonly IRedisClient _redis;
    private readonly ISignalingNotifier _notifier;
    private readonly ILogger<SessionNotificationService> _logger;
    private string _position = "0-0";

    public SessionNotificationService(IRedisClient redis, ISignalingNotifier notifier,
        ILogger<SessionNotificationService> logger)
    {
        _redis = redis;
        _notifier = notifier;
        _logger = logger;
    }

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        _position = await _redis.GetStreamPositionAsync(SignalingRedisKeys.SessionEvents, cancellationToken);
        await base.StartAsync(cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var entries = await _redis.ReadStreamAsync<RedisSessionRecord>(
                        SignalingRedisKeys.SessionEvents, "session", _position, 100, stoppingToken);
                    foreach (var entry in entries)
                    {
                        if (entry.Value is { } record)
                        {
                            RemoteSession session;
                            try
                            {
                                session = record.ToDomain();
                            }
                            catch (ValidationException error)
                            {
                                _logger.LogError(error, "Пропущена некорректная запись сессии {EntryId}", entry.Id);
                                _position = entry.Id;
                                continue;
                            }
                            if (session.State == SessionState.Pending)
                                await _notifier.IncomingRequest(session);
                            else
                                await _notifier.SessionUpdated(session);
                        }
                        _position = entry.Id;
                    }
                    if (entries.Count == 0) await Task.Delay(TimeSpan.FromMilliseconds(100), stoppingToken);
                }
                catch (Exception error) when (error is not OperationCanceledException)
                {
                    _logger.LogError(error, "Не удалось отправить изменения сессий");
                    await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
                }
            }
        }
        catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested) { }
    }
}
