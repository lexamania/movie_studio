using MediatR;

using MovieStudio.Shared.Application.DTOs;

namespace MovieStudio.Shared.Application.Queries.Handlers;

public interface IGetMovieByIdHandler : IRequestHandler<GetMovieByIdQuery, MovieFullDto>
{
    
}
