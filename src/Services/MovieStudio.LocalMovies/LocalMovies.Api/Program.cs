using LocalMovies.Api.Endpoints;
using LocalMovies.Api.Middleware;
using LocalMovies.Infrastructure.Configurations;
using LocalMovies.Infrastructure.Helpers;

using MovieStudio.Application.Configurations;

var builder = WebApplication.CreateBuilder(args);

// Configure logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Information);

var workingDir = Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MovieStudio");
DirectoryHelper.CreateDirectoryIfNeed(workingDir);

// Add shared application services (CQRS, validation, logging)
builder.Services.AddnMediatRExtended();

// Add infrastructure services
builder.Services.AddLocalServices(new ()
{
    WorkingDirectory = workingDir,
    SupportedExtensions = [ ".mp4", ".mkv" ]
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Use global exception handling middleware
app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.MapGet("/", () => Results.LocalRedirect("/swagger"));
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapEndpoints();
app.MapDirectoryEndpoints();

app.Run();
