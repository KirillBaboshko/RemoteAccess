using Microsoft.Extensions.DependencyInjection;
using RemoteAccess.Data.DataProviders.Users.Model;
using FluentValidation;

namespace RemoteAccess.Application;

/// <summary>
/// Расширения для регистрации сервисов Application слоя
/// </summary>
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Регистрация MediatR
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(ServiceCollectionExtensions).Assembly);
            cfg.RegisterServicesFromAssembly(typeof(UsersMappingProfile).Assembly);
        });
        
        // Регистрация FluentValidation
        services
            .AddValidatorsFromAssemblies([
            typeof(ServiceCollectionExtensions).Assembly,
                typeof(UsersMappingProfile).Assembly
        ]);

        ValidatorOptions.Global.DefaultRuleLevelCascadeMode = CascadeMode.Stop;

        // Регистрация AutoMapper (с версии 13.0 метод встроен в основной пакет)
        services.AddAutoMapper(cfg => { }, typeof(UsersMappingProfile).Assembly);

        return services;
    }
}
