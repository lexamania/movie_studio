
using LocalMovies.Infrastructure.Services;

using MediatR;

namespace LocalMovies.Api.Application.Commands;

public class RemoveDirectoryHandler(StorageService storage) : IRequestHandler<RemoveDirectoryCommand, bool>
{
    public Task<bool> Handle(RemoveDirectoryCommand request, CancellationToken cancellationToken)
    {
        storage.RemoveDirectory(request.Id);
        return Task.FromResult(true);
    }
}
