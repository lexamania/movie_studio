using MediatR;

namespace LocalMovies.Server.Api.Application.Commands;

public record AddDirectoryCommand(string DirectoryPath, string Caption, bool IncludeInner) : IRequest<bool>;
