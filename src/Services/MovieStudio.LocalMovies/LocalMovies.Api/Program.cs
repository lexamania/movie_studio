using LocalMovies.Api.Endpoints;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapReadEndpoints();
app.MapWriteEndpoints();

app.Run();
