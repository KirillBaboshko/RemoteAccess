using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RemoteAccess.Common.Redis.Abstractions;
using StackExchange.Redis;

namespace RemoteAccess.Common.Redis;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRedisClient(this IServiceCollection services, Action<RedisClientSettings> configure)
    {
        var settings = new RedisClientSettings();
        configure(settings);
        new RedisClientSettingsValidator().ValidateAndThrow(settings);

        services.AddSingleton<IOptions<RedisClientSettings>>(Options.Create(settings));
        services.AddSingleton<IConnectionMultiplexer>(_ =>
        {
            var options = ConfigurationOptions.Parse(settings.Configuration);
            options.AbortOnConnectFail = false;
            return ConnectionMultiplexer.Connect(options);
        });
        services.AddSingleton<IRedisClient, RedisClient>();
        return services;
    }
}
