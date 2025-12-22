using MediatR;

using MovieStudio.Application.DTOs;

namespace MovieStudio.Application.Queries.Handlers;

public interface IGetMoviesByCategoryHandler : IRequestHandler<GetMoviesByCategoryQuery, List<MovieShortDto>>
{
    
}
