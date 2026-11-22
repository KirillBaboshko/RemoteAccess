using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using RemoteAccess.Common.Application.Behaviors;

namespace RemoteAccess.Signaling.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(options =>
        {
            options.RegisterServicesFromAssembly(typeof(ServiceCollectionExtensions).Assembly);
            options.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });
        services.AddValidatorsFromAssembly(typeof(ServiceCollectionExtensions).Assembly);
        services.AddAutoMapper(typeof(ServiceCollectionExtensions).Assembly);
        ValidatorOptions.Global.DefaultRuleLevelCascadeMode = CascadeMode.Stop;
        return services;
    }
}
