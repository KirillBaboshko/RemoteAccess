using Microsoft.Extensions.DependencyInjection;
using RemoteAccess.Data.DataProviders.Users.Model;
using FluentValidation;
using RemoteAccess.Common.Application.Behaviors;

namespace RemoteAccess.Application;

/// <summary>
/// Расширения для регистрации сервисов Application слоя
/// </summary>
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(ServiceCollectionExtensions).Assembly);
            cfg.RegisterServicesFromAssembly(typeof(UsersMappingProfile).Assembly);
            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });
        
        services
            .AddValidatorsFromAssemblies([
            typeof(ServiceCollectionExtensions).Assembly,
                typeof(UsersMappingProfile).Assembly
        ]);

        ValidatorOptions.Global.DefaultRuleLevelCascadeMode = CascadeMode.Stop;

        services.AddAutoMapper(cfg => { }, typeof(UsersMappingProfile).Assembly);

        return services;
    }
}
