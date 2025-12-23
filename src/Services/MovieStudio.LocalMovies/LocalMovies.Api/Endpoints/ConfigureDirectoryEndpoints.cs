using LocalMovies.Api.Application.Commands;
using LocalMovies.Api.Endpoints.Models;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using MovieStudio.Application.Queries;

namespace LocalMovies.Api.Endpoints;

public static class ConfigureDirectoryEndpoints
{
    public static RouteGroupBuilder MapDirectoryEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/directories");
        group.MapGet("/", GetDirectories);
        group.MapPost("/", AddDirectory);
        group.MapDelete("/{id}", RemoveDirectory);
        return group;
    }

    public static async Task<IResult> GetDirectories(IMediator mediator)
    {
        var result = await mediator.Send(new GetCategoriesQuery());
        return Results.Ok(result);
    }

    public static async Task<IResult> AddDirectory([FromBody] NewDirectoryModel model, IMediator mediator)
    {
        await mediator.Send(new AddDirectoryCommand(model.DirectoryPath, model.Caption, model.IncludeInner));
        return Results.Created();
    }

    public static async Task<IResult> RemoveDirectory([FromRoute] string id, IMediator mediator)
    {
        var result = await mediator.Send(new RemoveDirectoryCommand(id));
        return Results.Ok(result);
    }
}