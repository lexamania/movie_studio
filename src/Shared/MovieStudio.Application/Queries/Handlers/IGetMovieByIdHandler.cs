using MediatR;

using MovieStudio.Application.DTOs;

namespace MovieStudio.Application.Queries.Handlers;

public interface IGetMovieByIdHandler : IRequestHandler<GetMovieByIdQuery, MovieFullDto>
{
    
}
