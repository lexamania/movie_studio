
using LocalMovies.Infrastructure.Services;

using MediatR;

namespace LocalMovies.Server.Api.Application.Commands;

public class AddDirectoryHandler(StorageService storage) : IRequestHandler<AddDirectoryCommand, bool>
{
    public Task<bool> Handle(AddDirectoryCommand request, CancellationToken cancellationToken)
    {
        storage.AddDirectory(request.DirectoryPath, request.Caption, request.IncludeInner);
        return Task.FromResult(true);
    }
}
