using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RemoteAccess.Domain.Repositories;
using RemoteAccess.Domain.Services.Token;
using RemoteAccess.Infrastructure.Database;
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

        services.AddOptions<JwtOptions>().BindConfiguration(JwtOptions.SectionName);
        services.AddSingleton<ITokenIssuer, JwtTokenIssuer>();

        // Регистрация репозиториев
        services.AddScoped<IUsersRepository, UsersRepository>();

        return services;
    }
}
