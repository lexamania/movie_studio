using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using MovieStudio.Application.Behaviors;

namespace MovieStudio.Application.Configurations;

public static class ConfigureServices
{
    public static IServiceCollection AddnMediatRExtended(this IServiceCollection services)
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();

        // Register MediatR with all handlers and behaviors
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblies(assemblies);
            
            // Register pipeline behaviors
            cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        // Register all validators
        services.AddValidatorsFromAssemblies(assemblies);

        return services;
    }
}
