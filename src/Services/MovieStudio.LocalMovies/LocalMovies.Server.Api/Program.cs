using LocalMovies.Server.Api.Endpoints;
using LocalMovies.Infrastructure.Configurations;
using LocalMovies.Infrastructure.Helpers;

using MovieStudio.Shared.Application.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Information);

var workingDir = Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MovieStudio");
DirectoryHelper.CreateDirectoryIfNeed(workingDir);

builder.Services.AddMediatRExtended();
builder.Services.AddLocalServices(new ()
{
    WorkingDirectory = workingDir,
    SupportedExtensions = [ ".mp4", ".mkv" ]
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseGlobalExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapGet("/", () => Results.LocalRedirect("/swagger"));
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapDirectoryEndpoints();

app.Run();
