using MediatR;

using MovieStudio.Shared.Application.DTOs;

namespace LocalMovies.Server.Api.Application.Queries;

public record GetDirectoriesQuery() : IRequest<List<CategoryDto>>;
