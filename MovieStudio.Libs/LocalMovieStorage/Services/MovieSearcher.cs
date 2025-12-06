using System.Collections.Generic;

using MovieStudio.Infrastructure.Movies.Interfaces;
using MovieStudio.Infrastructure.Movies.Models;
using MovieStudio.Infrastructure.Pagination.Models;

namespace LocalMovieStorage.Services;

public class MovieSearcher : IMovieSearcher
{
    public List<MovieShortDto> Search(string name, PageModel page)
    {
        throw new System.NotImplementedException();
    }
}
