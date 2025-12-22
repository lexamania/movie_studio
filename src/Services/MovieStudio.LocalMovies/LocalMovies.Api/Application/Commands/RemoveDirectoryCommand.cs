using MediatR;

namespace LocalMovies.Api.Application.Commands;

public record RemoveDirectoryCommand(string Id) : IRequest<bool>;
