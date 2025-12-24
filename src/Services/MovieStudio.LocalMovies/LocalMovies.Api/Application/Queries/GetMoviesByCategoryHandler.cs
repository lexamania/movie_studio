using LocalMovies.Infrastructure.Services;

using MovieStudio.Shared.Application.DTOs;
using MovieStudio.Shared.Application.Queries;
using MovieStudio.Shared.Application.Queries.Handlers;
using MovieStudio.Shared.Domain.Extensions;

namespace LocalMovies.Api.Application.Queries;

public class GetMoviesByCategoryHandler(StorageService storage) : IGetMoviesByCategoryHandler
{
    public Task<List<MovieShortDto>> Handle(GetMoviesByCategoryQuery request, CancellationToken cancellationToken)
    {
        var dirs = storage.GetDirectories();
        var dir = dirs.FirstOrDefault(x => request.CategoryId.Equals(x.Directory.Id));
        if (dir is null)
            return Task.FromResult(new List<MovieShortDto>());

        var movies = dir.Files.WithPagination(request.Pagination).Select(x => new MovieShortDto()
        {
            Id = x,
            Title = Path.GetFileNameWithoutExtension(x)
        }).ToList();

        return Task.FromResult(movies);
    }
}
