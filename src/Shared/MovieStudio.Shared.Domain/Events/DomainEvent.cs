using MediatR;

namespace MovieStudio.Shared.Domain.Events;

/// <summary>
/// Base domain event - Enables event-driven architecture
/// Allows handlers to react to domain events asynchronously
/// </summary>
public abstract class DomainEvent : INotification
{
    public DateTime OccurredAt { get; } = DateTime.UtcNow;
    public Guid EventId { get; } = Guid.NewGuid();
}
