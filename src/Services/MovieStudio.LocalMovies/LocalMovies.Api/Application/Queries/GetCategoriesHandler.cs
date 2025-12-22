using LocalMovies.Infrastructure.Services;

using MovieStudio.Application.DTOs;
using MovieStudio.Application.Queries;
using MovieStudio.Application.Queries.Handlers;

namespace LocalMovies.Api.Application.Queries;

public class GetCategoriesHandler(StorageService storage) : IGetCategoriesHandler
{
    public Task<List<CategoryDto>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        var dirs = storage.GetDirectories();
        var categories = dirs.Select(x => new CategoryDto()
        {
            Id = x.Directory.Id,
            Title = x.Directory.Caption
        }).ToList();

        return Task.FromResult(categories);
    }
}
