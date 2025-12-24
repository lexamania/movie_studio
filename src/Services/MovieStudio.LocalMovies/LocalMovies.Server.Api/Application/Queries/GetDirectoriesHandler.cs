using LocalMovies.Infrastructure.Services;

using MediatR;

using MovieStudio.Shared.Application.DTOs;

namespace LocalMovies.Server.Api.Application.Queries;

public class GetDirectoriesHandler(StorageService storage) : IRequestHandler<GetDirectoriesQuery, List<CategoryDto>>
{
    public Task<List<CategoryDto>> Handle(GetDirectoriesQuery request, CancellationToken cancellationToken)
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
