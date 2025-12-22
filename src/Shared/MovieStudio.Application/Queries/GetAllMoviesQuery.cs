using MediatR;

using MovieStudio.Application.DTOs;
using MovieStudio.Domain.Interfaces;
using MovieStudio.Domain.Models;

namespace MovieStudio.Application.Queries;

public record GetAllMoviesQuery(PaginationModel Pagination) : IRequest<List<MovieShortDto>>, IPaginated;