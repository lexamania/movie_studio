using FluentValidation;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using MovieStudio.Shared.Application.Behaviors;
using MovieStudio.Shared.Application.Middleware;

namespace MovieStudio.Shared.Application.Configurations;

public static class ConfigureServices
{
    public static IServiceCollection AddMediatRExtended(this IServiceCollection services)
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

    public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder services)
        => services.UseMiddleware<GlobalExceptionHandlingMiddleware>();
}
