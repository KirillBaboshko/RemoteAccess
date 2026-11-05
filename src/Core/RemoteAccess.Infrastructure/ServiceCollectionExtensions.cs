using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RemoteAccess.Common.Data;
using RemoteAccess.Data.DataProviders.Users;
using RemoteAccess.Domain.Repositories;
using RemoteAccess.Domain.Services.Token;
using RemoteAccess.Infrastructure.Database;
using RemoteAccess.Infrastructure.DataProviders;
using RemoteAccess.Infrastructure.Repositories;
using RemoteAccess.Infrastructure.Services;

namespace RemoteAccess.Infrastructure;

/// <summary>
/// Расширения для регистрации сервисов Infrastructure слоя
/// </summary>
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
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
        //services.AddScoped<IDeviceDataProvider, DevicesDataProvider>();

        return services;
    }
}
