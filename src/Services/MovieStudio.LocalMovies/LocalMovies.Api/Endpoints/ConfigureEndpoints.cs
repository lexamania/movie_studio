using MediatR;

using Microsoft.AspNetCore.Mvc;

using MovieStudio.Application.Queries;
using MovieStudio.Domain.Models;

namespace LocalMovies.Api.Endpoints;

public static class ConfigureEndpoints
{
    public static void MapEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/movies");
        group.MapGet("/categories", GetCategories);
        group.MapGet("/categories/all", GetAllMovies);
        group.MapGet("/categories/{categoryId}", GetMoviesByCategory);
        group.MapGet("/{movieId}", GetMovieById);
    }

    public static async Task<IResult> GetCategories(IMediator mediator)
    {
        var result = await mediator.Send(new GetCategoriesQuery());
        return Results.Ok(result);
    }

    public static async Task<IResult> GetAllMovies(
        [AsParameters] PaginationModel pagination, 
        IMediator mediator)
    {
        var result = await mediator.Send(new GetAllMoviesQuery(pagination));
        return Results.Ok(result);
    }

    public static async Task<IResult> GetMoviesByCategory(
        [FromRoute] string categoryId,
        [AsParameters] PaginationModel pagination, 
        IMediator mediator)
    {
        var result = await mediator.Send(new GetMoviesByCategoryQuery(categoryId, pagination));
        return Results.Ok(result);
    }

    public static async Task<IResult> GetMovieById(
        [FromRoute] string movieId, 
        IMediator mediator)
    {
        var result = await mediator.Send(new GetMovieByIdQuery(movieId));
        return Results.Ok(result);
    }
}