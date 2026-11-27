using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RemoteAccess.Common.Data;
using RemoteAccess.Data.DataProviders.Users;
using RemoteAccess.Data.DataProviders.Devices;
using RemoteAccess.Domain.Repositories;
using RemoteAccess.Domain.Services.Token;
using RemoteAccess.Infrastructure.Database;
using RemoteAccess.Infrastructure.DataProviders;
using RemoteAccess.Infrastructure.Repositories;
using RemoteAccess.Infrastructure.Services;
using RemoteAccess.Infrastructure.Clients;
using RemoteAccess.Infrastructure.Settings;
using RemoteAccess.Contracts.Signaling.Devices;
using RemoteAccess.Contracts.Signaling.Sessions;
using RemoteAccess.Data.DataProviders.Sessions;
using RemoteAccess.Domain.Models.Ecxeptions;
using Microsoft.Extensions.Options;

namespace RemoteAccess.Infrastructure;

/// <summary>
/// Расширения для регистрации сервисов Infrastructure слоя
/// </summary>
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddOptions<SignalingApiSettings>().BindConfiguration("Signaling")
            .Validate(settings => settings.BaseUrl is { IsAbsoluteUri: true, Scheme: "http" or "https" }
                && settings.Timeout > TimeSpan.Zero,
                ValidationErrorMessages.SignalingSettingsInvalid).ValidateOnStart();
        services.AddGrpcClient<DeviceAvailability.DeviceAvailabilityClient>((provider, options) =>
            options.Address = provider.GetRequiredService<IOptions<SignalingApiSettings>>().Value.BaseUrl);
        services.AddScoped<IDeviceAvailabilityProvider, SignalingDeviceAvailabilityProvider>();
        services.AddGrpcClient<SessionHistory.SessionHistoryClient>((provider, options) =>
            options.Address = provider.GetRequiredService<IOptions<SignalingApiSettings>>().Value.BaseUrl);
        services.AddScoped<ISessionHistoryProvider, SignalingSessionHistoryProvider>();

        // Регистрация DbContext (InMemory для упрощения)
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseInMemoryDatabase("RemoteAccessDb");
        });

        // Регистрация JWT
        services.AddOptions<JwtOptions>().BindConfiguration(JwtOptions.SectionName);
        services.AddSingleton<ITokenIssuer, JwtTokenIssuer>();

        // Регистрация UnitOfWork
        services.AddScoped<IUnitOfWork, Infrastructure.Data.UnitOfWork>();

        // Регистрация репозиториев (для записи - CQRS Write Side)
        services.AddScoped<IUsersRepository, UsersRepository>();
        services.AddScoped<IDevicesRepository, DevicesRepository>();

        // Регистрация DataProviders (для чтения - CQRS Read Side)
        services.AddScoped<IUserDataProvider, UsersDataProvider>();
        services.AddScoped<IDeviceDataProvider, DevicesDataProvider>();

        return services;
    }
}
