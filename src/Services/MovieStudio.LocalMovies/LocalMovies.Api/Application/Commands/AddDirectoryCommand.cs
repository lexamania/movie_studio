using MediatR;

namespace LocalMovies.Api.Application.Commands;

public record AddDirectoryCommand(string DirectoryPath, string Caption, bool IncludeInner) : IRequest<bool>;
