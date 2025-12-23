using LocalMovies.Infrastructure.Services;

using MovieStudio.Application.DTOs;
using MovieStudio.Application.Queries;
using MovieStudio.Application.Queries.Handlers;

namespace LocalMovies.Api.Application.Queries;

public class GetMovieByIdHandler(StorageService storage) : IGetMovieByIdHandler
{
    public Task<MovieFullDto> Handle(GetMovieByIdQuery request, CancellationToken cancellationToken)
    {
        var dirs = storage.GetDirectories();
        var moviePath = dirs.SelectMany(d => d.Files).FirstOrDefault(f => f.Equals(request.MovieId));

        if (moviePath is null)
            return Task.FromResult<MovieFullDto>(null!);

        var dto = new MovieFullDto
        {
            Id = moviePath,
            Title = Path.GetFileNameWithoutExtension(moviePath),
            Description = "Movie description",
            ImageLink = string.Empty,
            VideoLink = moviePath
        };

        return Task.FromResult(dto);
    }
}
