using LocalMovies.Infrastructure.Services;

using MovieStudio.Application.DTOs;
using MovieStudio.Application.Queries;
using MovieStudio.Application.Queries.Handlers;

namespace LocalMovies.Api.Application.Queries;

public class GetMovieByIdHandler(StorageService storage) : IGetMovieByIdHandler
{
    public Task<MovieFullDto> Handle(GetMovieByIdQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
