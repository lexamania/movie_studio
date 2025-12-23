using LocalMovies.Infrastructure.Events;
using MediatR;

namespace LocalMovies.Api.Application.EventHandlers;

/// <summary>
/// Example domain event handler - demonstrates event-driven architecture
/// This can be extended to publish events to message queues (RabbitMQ, Kafka, etc.)
/// for inter-service communication
/// </summary>
public class DirectoryEventHandler :
    INotificationHandler<DirectoryAddedEvent>,
    INotificationHandler<DirectoryRemovedEvent>,
    INotificationHandler<MoviesDiscoveredEvent>
{
    private readonly ILogger<DirectoryEventHandler> _logger;

    public DirectoryEventHandler(ILogger<DirectoryEventHandler> logger)
        => _logger = logger;

    public Task Handle(DirectoryAddedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Directory added event: {DirectoryId} at {DirectoryPath}",
            notification.DirectoryId,
            notification.DirectoryPath);
        
        // TODO: Publish to message queue for other services to consume
        // e.g., master service could be notified about new movie sources
        
        return Task.CompletedTask;
    }

    public Task Handle(DirectoryRemovedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Directory removed event: {DirectoryId} from {DirectoryPath}",
            notification.DirectoryId,
            notification.DirectoryPath);
        
        // TODO: Publish to message queue to notify other services
        
        return Task.CompletedTask;
    }

    public Task Handle(MoviesDiscoveredEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Movies discovered event: {MovieCount} movies in directory {DirectoryId}",
            notification.MovieCount,
            notification.DirectoryId);
        
        // TODO: Publish to message queue for indexing or caching services
        
        return Task.CompletedTask;
    }
}
