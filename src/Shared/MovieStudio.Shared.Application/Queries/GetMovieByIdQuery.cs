using MediatR;

using MovieStudio.Shared.Application.DTOs;

namespace MovieStudio.Shared.Application.Queries;

public record GetMovieByIdQuery(string MovieId) : IRequest<MovieFullDto>;
