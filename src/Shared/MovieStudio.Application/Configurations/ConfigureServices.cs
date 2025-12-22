using Microsoft.Extensions.DependencyInjection;

namespace MovieStudio.Application.Configurations;

public static class ConfigureServices
{
    public static IServiceCollection AddInjectionSharedApplication(this IServiceCollection services)
        => services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));
}
