using MovieStudio.Shared.Domain.Events;

namespace LocalMovies.Infrastructure.Events;

public class DirectoryAddedEvent : DomainEvent
{
    public string DirectoryId { get; set; } = string.Empty;
    public string DirectoryPath { get; set; } = string.Empty;
    public string Caption { get; set; } = string.Empty;
}

public class DirectoryRemovedEvent : DomainEvent
{
    public string DirectoryId { get; set; } = string.Empty;
    public string DirectoryPath { get; set; } = string.Empty;
}

public class MoviesDiscoveredEvent : DomainEvent
{
    public string DirectoryId { get; set; } = string.Empty;
    public List<string> MoviePaths { get; set; } = [];
    public int MovieCount { get; set; }
}
