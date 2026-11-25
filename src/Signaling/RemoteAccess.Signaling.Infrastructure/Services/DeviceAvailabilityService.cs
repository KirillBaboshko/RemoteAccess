using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RemoteAccess.Common.Redis.Abstractions;
using RemoteAccess.Signaling.Clients.Signaling;
using RemoteAccess.Signaling.Clients.Signaling.Models;
using RemoteAccess.Signaling.Infrastructure.Redis;
using RemoteAccess.Signaling.Infrastructure.Redis.Models;

namespace RemoteAccess.Signaling.Infrastructure.Services;

public sealed class DeviceAvailabilityService : BackgroundService
{
    private readonly IRedisClient _redis;
    private readonly ISignalingNotifier _notifier;
    private readonly ILogger<DeviceAvailabilityService> _logger;
    private string _position = "0-0";

    public DeviceAvailabilityService(IRedisClient redis, ISignalingNotifier notifier,
        ILogger<DeviceAvailabilityService> logger)
    {
        _redis = redis;
        _notifier = notifier;
        _logger = logger;
    }

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        _position = await _redis.GetStreamPositionAsync(SignalingRedisKeys.AvailabilityEvents, cancellationToken);
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
                    var entries = await _redis.ReadStreamAsync<RedisAvailabilityChange>(
                        SignalingRedisKeys.AvailabilityEvents, "change", _position, 100, stoppingToken);
                    foreach (var entry in entries)
                    { 
                        if (entry.Value is { } change)
                            await _notifier.DeviceAvailabilityChanged(new(change.DeviceIds, entry.Id));
                        _position = entry.Id;
                    }
                    if (entries.Count == 0) await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
                }
                catch (Exception error) when (error is not OperationCanceledException)
                {
                    _logger.LogError(error, "Не удалось отправить изменения доступности устройств");
                    await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
                }
            }
        }
        catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested) { }
    }
}
