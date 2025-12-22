using LocalMovies.Api.Endpoints;
using LocalMovies.Infrastructure.Configurations;
using LocalMovies.Infrastructure.Helpers;

using MovieStudio.Application.Configurations;

var builder = WebApplication.CreateBuilder(args);

var workingDir = Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MovieStudio");
DirectoryHelper.CreateDirectoryIfNeed(workingDir);

builder.Services.AddInjectionSharedApplication();
builder.Services.AddServicesConfiguration(new ()
{
    WorkingDirectory = workingDir,
    SupportedExtensions = [ ".mp4", ".mkv" ]
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapGet("/", () => Results.LocalRedirect("/swagger"));
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapEndpoints();
app.MapDirectoryEndpoints();

app.Run();
