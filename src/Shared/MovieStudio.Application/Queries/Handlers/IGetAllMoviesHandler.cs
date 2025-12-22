using MediatR;

using MovieStudio.Application.DTOs;

namespace MovieStudio.Application.Queries.Handlers;

public interface IGetAllMoviesHandler : IRequestHandler<GetAllMoviesQuery, List<MovieShortDto>>
{
    
}
