using MovieStudio.Infrastructure.Movies.Models;

namespace MovieStudio.Infrastructure.Movies.Interfaces;

public interface IMovieReader
{
    public MovieFullDto GetMovieInfoById(string id);
}