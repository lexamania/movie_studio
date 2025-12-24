using MediatR;

namespace LocalMovies.Server.Api.Application.Commands;

public record RemoveDirectoryCommand(string Id) : IRequest<bool>;
