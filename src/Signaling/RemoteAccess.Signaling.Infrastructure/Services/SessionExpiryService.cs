using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RemoteAccess.Signaling.Application.Features.Sessions.Commands;
using RemoteAccess.Signaling.Domain.Repositories;
using RemoteAccess.Signaling.Application.Features.Sessions.Models;

namespace RemoteAccess.Signaling.Infrastructure.Services;

public sealed class SessionExpiryService : BackgroundService
{
    private readonly IServiceScopeFactory _scopes;
    private readonly ILogger<SessionExpiryService> _logger;
    private readonly ISignalingState _state;
    private readonly SessionOptions _options;

    public SessionExpiryService(IServiceScopeFactory scopes, ILogger<SessionExpiryService> logger,
        ISignalingState state, IOptions<SessionOptions> options)
    {
        _scopes = scopes;
        _logger = logger;
        _state = state;
        _options = options.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var timer = new PeriodicTimer(_options.MaintenanceInterval);
        try
        {
            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                try
                {
                    using var scope = _scopes.CreateScope();
                    await _state.RenewLeasesAsync(stoppingToken);
                    await scope.ServiceProvider.GetRequiredService<ISender>().Send(new ExpireSessionsCommand(), stoppingToken);
                }
                catch (Exception error) when (error is not OperationCanceledException)
                {
                    _logger.LogError(error, "Не удалось обработать истечение запросов подключения");
                }
            }
        }
        catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
        {
        }
    }
}
