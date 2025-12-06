using MovieStudio.Infrastructure.Movies.Interfaces;
using MovieStudio.Infrastructure.Movies.Models;

namespace LocalMovieStorage.Services;

public class MovieService : IMovieReader
{
    public MovieFullDto GetMovieInfoById(string id)
    {
        throw new System.NotImplementedException();
    }
}
