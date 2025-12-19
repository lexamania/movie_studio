namespace LocalMovies.Api.Endpoints;

public static class ReadEndpoints
{
    public static void MapReadEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/movies");
        group.MapGet("/categories", () => "Hello World!");
        group.MapGet("/all", () => "Hello World!");
        group.MapGet("/category/{category}", (string category) => "Hello World!");
    
        group.MapPost("/add", () => "Hello World!");
        group.MapPost("/", () => "Hello World!");
        
    }
}