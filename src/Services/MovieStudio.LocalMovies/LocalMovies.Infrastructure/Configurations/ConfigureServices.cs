using LocalMovies.Infrastructure.Interfaces;
using LocalMovies.Infrastructure.Parsers;
using LocalMovies.Infrastructure.Services;

using Microsoft.Extensions.DependencyInjection;

namespace LocalMovies.Infrastructure.Configurations;

public static class ConfigureServices
{
    public static IServiceCollection AddServicesConfiguration(this IServiceCollection services, MovieStorageSettings settings)
    {
        services.AddSingleton(settings);
        services.AddSingleton<IFileParser, CsvParser>();
        services.AddSingleton<StorageService>();
        return services;
    }
}
