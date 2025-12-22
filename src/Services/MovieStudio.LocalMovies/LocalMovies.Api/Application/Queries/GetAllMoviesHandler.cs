using LocalMovies.Infrastructure.Services;

using MovieStudio.Application.DTOs;
using MovieStudio.Application.Queries;
using MovieStudio.Application.Queries.Handlers;
using MovieStudio.Domain.Extensions;

namespace LocalMovies.Api.Application.Queries;

public class GetAllMoviesHandler(StorageService storage) : IGetAllMoviesHandler
{
    public Task<List<MovieShortDto>> Handle(GetAllMoviesQuery request, CancellationToken cancellationToken)
    {
        var dirs = storage.GetDirectories().WithPagination(request.Pagination).ToList();
        var movies = dirs.SelectMany(x => x.Files).Select(x => new MovieShortDto()
        {
            Id = x,
            Title = Path.GetFileNameWithoutExtension(x)
        }).ToList();
        return Task.FromResult(movies);
    }
}
