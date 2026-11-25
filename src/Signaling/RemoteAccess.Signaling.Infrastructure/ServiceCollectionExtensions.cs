using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RemoteAccess.Common.Data;
using RemoteAccess.Common.Redis;
using RemoteAccess.Contracts.Core.Devices;
using RemoteAccess.Signaling.Application.Features.Sessions.Models;
using RemoteAccess.Signaling.Clients.Devices;
using RemoteAccess.Signaling.Clients.Signaling;
using RemoteAccess.Signaling.Domain.Repositories;
using RemoteAccess.Signaling.Infrastructure.Clients;
using RemoteAccess.Signaling.Infrastructure.Database;
using RemoteAccess.Signaling.Infrastructure.Redis;
using RemoteAccess.Signaling.Infrastructure.Repositories;
using RemoteAccess.Signaling.Infrastructure.Services;
using RemoteAccess.Signaling.Infrastructure.Settings;
using StackExchange.Redis;

namespace RemoteAccess.Signaling.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSignalingNotifier<THub>(this IServiceCollection services)
        where THub : Hub<ISignalingClient>
    {
        return services.AddSingleton<ISignalingNotifier, SignalingNotifier<THub>>();
    }

    public static IServiceCollection AddAndConfigureInfrastructure(this IServiceCollection services, InfrastructureSettings settings)
    {
        settings.Validate();
        return services.AddAndConfigureRedis(settings.RedisSettings)
            .AddAndConfigureDatabase(settings.DatabaseName)
            .AddAndConfigureGrpcClients(settings.CoreSettings)
            .AddRepositories()
            .AddServices(settings.SessionSettings);
    }

    private static IServiceCollection AddAndConfigureRedis(this IServiceCollection services, RedisClientSettings settings)
    {
        services.AddRedisClient(options =>
        {
            options.Configuration = settings.Configuration;
            options.InstanceName = settings.InstanceName;
        });
        services.AddSignalR().AddStackExchangeRedis();
        services.AddOptions<Microsoft.AspNetCore.SignalR.StackExchangeRedis.RedisOptions>()
            .Configure<IConnectionMultiplexer, IOptions<RedisClientSettings>>((options, redis, clientSettings) =>
            {
                options.ConnectionFactory = _ => Task.FromResult(redis);
                options.Configuration.ChannelPrefix = RedisChannel.Literal(clientSettings.Value.InstanceName + SignalingRedisKeys.Backplane);
            });
        return services;
    }

    private static IServiceCollection AddAndConfigureDatabase(this IServiceCollection services, string databaseName)
    {
        services.AddDbContext<SignalingDbContext>(options => options.UseInMemoryDatabase(databaseName));
        services.AddScoped<IUnitOfWork>(provider => new UnitOfWork(provider.GetRequiredService<SignalingDbContext>()));
        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<ISessionHistoryRepository, SessionHistoryRepository>();
        services.AddSingleton<ISignalingState, RedisSignalingState>();
        return services;
    }

    private static IServiceCollection AddAndConfigureGrpcClients(this IServiceCollection services, CoreApiSettings settings)
    {
        services.AddSingleton<IOptions<CoreApiSettings>>(Options.Create(settings));
        services.AddScoped<IDeviceAccessProvider, CoreDeviceAccessProvider>();
        services.AddGrpcClient<DeviceAccess.DeviceAccessClient>(options => options.Address = settings.BaseUrl);
        return services;
    }

    private static IServiceCollection AddServices(this IServiceCollection services, SessionOptions sessionSettings)
    {
        services.AddSingleton<IOptions<SessionOptions>>(Options.Create(sessionSettings));
        services.AddSingleton<LocalConnections>();
        services.AddHostedService<SessionExpiryService>();
        services.AddHostedService<SessionHistoryService>();
        return services;
    }
}
