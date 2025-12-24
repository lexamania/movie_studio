using MediatR;

using MovieStudio.Shared.Application.DTOs;
using MovieStudio.Shared.Domain.Interfaces;
using MovieStudio.Shared.Domain.Models;

namespace MovieStudio.Shared.Application.Queries;

public record GetMoviesByCategoryQuery(string CategoryId, PaginationModel Pagination)
    : IRequest<List<MovieShortDto>>, IPaginated;
