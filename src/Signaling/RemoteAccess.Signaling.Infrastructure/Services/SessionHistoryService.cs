using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RemoteAccess.Common.Data;
using RemoteAccess.Common.Redis.Abstractions;
using RemoteAccess.Signaling.Domain.Repositories;
using RemoteAccess.Signaling.Infrastructure.Redis;
using RemoteAccess.Signaling.Infrastructure.Redis.Models;

namespace RemoteAccess.Signaling.Infrastructure.Services;

public sealed class SessionHistoryService : BackgroundService
{
    private readonly IRedisClient _redis;
    private readonly IServiceScopeFactory _scopes;
    private readonly ILogger<SessionHistoryService> _logger;

    public SessionHistoryService(IRedisClient redis,
        IServiceScopeFactory scopes, ILogger<SessionHistoryService> logger)
    {
        _redis = redis;
        _scopes = scopes;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var position = "0-0";
        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var entries = await _redis.ReadStreamAsync<RedisSessionRecord>(
                        SignalingRedisKeys.SessionEvents, "session", position, 100, stoppingToken);
                    foreach (var entry in entries)
                    {
                        using var scope = _scopes.CreateScope();
                        var record = entry.Value;
                        await scope.ServiceProvider.GetRequiredService<ISessionHistoryRepository>()
                            .SaveAsync(record.ToDomain(), stoppingToken);
                        await scope.ServiceProvider.GetRequiredService<IUnitOfWork>().SaveChangesAsync(stoppingToken);
                        position = entry.Id;
                        _logger.LogDebug("Сохранена история сессии {SessionId}: {State}, версия {Version}",
                            record.Id, record.State, record.Version);
                    }
                    if (entries.Count == 0) await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
                }
                catch (Exception error) when (error is not OperationCanceledException)
                {
                    _logger.LogError(error, "Не удалось сохранить историю сессий");
                    await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
                }
            }
        }
        catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested) { }
    }
}
