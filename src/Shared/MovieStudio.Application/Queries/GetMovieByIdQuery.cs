using MediatR;

using MovieStudio.Application.DTOs;

namespace MovieStudio.Application.Queries;

public record GetMovieByIdQuery(string MovieId) : IRequest<MovieFullDto>;
