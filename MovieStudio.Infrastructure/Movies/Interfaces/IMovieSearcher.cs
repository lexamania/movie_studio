using MovieStudio.Infrastructure.Movies.Models;
using MovieStudio.Infrastructure.Pagination.Models;

namespace MovieStudio.Infrastructure.Movies.Interfaces;

public interface IMovieSearcher
{
    public List<MovieShortDto> Search(string name, PageModel page);
}