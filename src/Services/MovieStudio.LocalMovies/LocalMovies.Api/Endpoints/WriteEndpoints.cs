namespace LocalMovies.Api.Endpoints;

public static class WriteEndpoints
{
    public static void MapWriteEndpoints(this WebApplication app)
    {
        app.MapPost("/", () => "Hello World!");
    }
}
