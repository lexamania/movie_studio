# Movie Studio - Microservice Architecture & CQRS Refactoring Guide

## Architecture Overview

This project implements a **microservice architecture** with **CQRS (Command Query Responsibility Segregation)** pattern and **event-driven architecture**.

### Project Structure

```
src/
├── Services/                    # Microservices
│   ├── MovieStudio.LocalMovies/   # Local file-based movie service
│   ├── MovieStudio.HBO/            # HBO movies service
│   └── MovieStudio.Netflix/        # Netflix movies service
└── Shared/                      # Shared libraries
    ├── MovieStudio.Application    # CQRS handlers, validators, behaviors
    └── MovieStudio.Domain         # Domain models, interfaces, events
```

## Key Patterns Implemented

### 1. CQRS Pattern (Command Query Responsibility Segregation)

**Separates read and write operations:**

#### Commands (Write Operations)
- `AddDirectoryCommand` - Creates new directory entry
- `RemoveDirectoryCommand` - Removes directory entry

Commands should:
- Mutate state
- Be validated
- Publish domain events
- Return minimal response (usually void or ID)

#### Queries (Read Operations)
- `GetAllMoviesQuery` - Retrieve all movies with pagination
- `GetMoviesByCategory` - Filter movies by category
- `GetMovieByIdQuery` - Get single movie details
- `GetCategoriesQuery` - Retrieve all categories

Queries should:
- Be read-only
- Not modify state
- Can be cached
- Include pagination for large datasets

#### MediatR Pipeline Behaviors

The CQRS handlers are orchestrated using MediatR's pipeline behavior pattern:

```
Request → LoggingBehavior → ValidationBehavior → Handler → Response
```

**Behaviors Implemented:**
1. **ValidationBehavior** - Validates requests using FluentValidation
2. **LoggingBehavior** - Logs all request/response execution

### 2. Domain-Driven Design (DDD)

**Domain Layer (`MovieStudio.Domain`):**
- `Models/` - Domain entities and value objects
- `Interfaces/` - Domain abstractions
- `Extensions/` - Domain logic extensions
- `Events/` - Domain events
- `Exceptions/` - Domain exceptions

**Key Classes:**
- `PaginationModel` - Pagination logic
- `DomainEvent` - Base class for all domain events
- `ValidationException` - CQRS validation errors

### 3. Event-Driven Architecture

Domain events enable asynchronous, decoupled communication:

```csharp
public abstract class DomainEvent : INotification
{
    public DateTime OccurredAt { get; }
    public Guid EventId { get; }
}
```

**Events Defined:**
- `DirectoryAddedEvent` - Fired when directory is added
- `DirectoryRemovedEvent` - Fired when directory is removed
- `MoviesDiscoveredEvent` - Fired when movies are indexed

**Usage:**
```csharp
// In handlers
var @event = new DirectoryAddedEvent 
{ 
    DirectoryId = id,
    DirectoryPath = path,
    Caption = caption
};

await mediator.Publish(@event);
```

### 4. Validation Pipeline

Automatic validation using FluentValidation and MediatR:

```csharp
public class AddDirectoryCommandValidator : AbstractValidator<AddDirectoryCommand>
{
    public AddDirectoryCommandValidator()
    {
        RuleFor(x => x.DirectoryPath)
            .NotEmpty()
            .Must(DirectoryExists);
        
        RuleFor(x => x.Caption)
            .NotEmpty()
            .MaximumLength(255);
    }
}
```

**How it works:**
1. Request arrives at handler
2. ValidationBehavior checks all registered validators
3. If validation fails, `ValidationException` is thrown
4. GlobalExceptionHandlingMiddleware catches it
5. Client receives 400 Bad Request with detailed error messages

### 5. Error Handling

**Global Exception Middleware:**
- Catches all unhandled exceptions
- Maps exceptions to appropriate HTTP status codes
- Returns consistent error responses

```json
{
    "message": "Validation failed",
    "timestamp": "2025-12-23T10:30:00Z",
    "errors": {
        "directoryPath": ["Directory path does not exist"]
    }
}
```

## Best Practices Applied

### 1. Separation of Concerns
- Commands vs Queries separated
- Handlers isolated from endpoints
- Infrastructure separated from application

### 2. Single Responsibility Principle
- Each handler has one job
- Each validator validates one request type
- Each behavior handles one cross-cutting concern

### 3. Open/Closed Principle
- Open for extension (add validators, behaviors, events)
- Closed for modification (handlers don't change)

### 4. Dependency Inversion
- Program depends on abstractions (IRequest, IRequestHandler)
- Behaviors and validators injected via DI
- Easy to swap implementations

## Extending the Architecture

### Adding a New Command

1. **Define Command** in `Application/Commands/`:
```csharp
public record UpdateMovieCommand(string Id, string Title) : IRequest<bool>;
```

2. **Create Validator** in `Application/Validators/`:
```csharp
public class UpdateMovieCommandValidator : AbstractValidator<UpdateMovieCommand>
{
    public UpdateMovieCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Title).NotEmpty().MaximumLength(255);
    }
}
```

3. **Implement Handler** in `Application/Commands/`:
```csharp
public class UpdateMovieHandler : IRequestHandler<UpdateMovieCommand, bool>
{
    public async Task<bool> Handle(UpdateMovieCommand request, CancellationToken ct)
    {
        // Update logic
        return true;
    }
}
```

4. **Publish Events** in handler:
```csharp
await mediator.Publish(new MovieUpdatedEvent { ... });
```

5. **Map Endpoint**:
```csharp
group.MapPut("/{id}", UpdateMovie);

public static async Task<IResult> UpdateMovie(
    [FromRoute] string id,
    [FromBody] UpdateMovieRequest request,
    IMediator mediator)
{
    var result = await mediator.Send(
        new UpdateMovieCommand(id, request.Title));
    return Results.Ok(result);
}
```

### Inter-Service Communication

Currently services are independent. To enable communication:

1. **Option A: Message Queue (Recommended for Production)**
   - Use RabbitMQ, Kafka, or Azure Service Bus
   - Publish domain events to queues
   - Other services subscribe to events

2. **Option B: HTTP REST**
   - Use HttpClient factory to call other services
   - Implement circuit breaker pattern

3. **Option C: gRPC**
   - Define service contracts
   - Efficient binary protocol

Example with message queue:
```csharp
// In handler
var @event = new DirectoryAddedEvent { ... };
await mediator.Publish(@event);

// EventHandler publishes to RabbitMQ
public class DirectoryEventHandler
{
    private readonly IMessagePublisher _publisher;
    
    public async Task Handle(DirectoryAddedEvent evt, CancellationToken ct)
    {
        await _publisher.PublishAsync("directory.added", evt);
    }
}

// Other service subscribes
public class MovieIndexer
{
    public async Task OnDirectoryAdded(DirectoryAddedEvent evt)
    {
        // Index movies from new directory
    }
}
```

## Database Considerations

Current implementation uses CSV files. For production:

1. **Use a Proper Database:**
   - SQL Server, PostgreSQL, MongoDB
   - Provides transactions, indexes, backups

2. **Add Unit of Work Pattern:**
```csharp
public class UnitOfWork
{
    public IMovieRepository Movies { get; }
    public IDirectoryRepository Directories { get; }
    
    public async Task SaveChangesAsync()
    {
        // Commit all changes
        // Publish domain events
    }
}
```

3. **Implement Repository Pattern:**
```csharp
public interface IMovieRepository
{
    Task<Movie?> GetByIdAsync(string id);
    Task<List<Movie>> GetByCategoryAsync(string categoryId);
    Task AddAsync(Movie movie);
}
```

## Testing

With this architecture, testing is straightforward:

### Unit Tests for Handlers
```csharp
[Test]
public async Task Handle_ValidCommand_ReturnsTrue()
{
    var handler = new AddDirectoryHandler(storageMock);
    var command = new AddDirectoryCommand("/path", "Caption", true);
    
    var result = await handler.Handle(command, CancellationToken.None);
    
    Assert.IsTrue(result);
}
```

### Integration Tests
```csharp
[Test]
public async Task Post_Directories_WithInvalidPath_Returns400()
{
    var response = await client.PostAsJsonAsync(
        "/directories",
        new { directoryPath = "/invalid/path" });
    
    Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
}
```

## Deployment Considerations

Each microservice can be deployed independently:

```yaml
# docker-compose.yml
services:
  local-movies:
    image: localmovies:latest
    ports:
      - "5133:5133"
  
  hbo:
    image: hbo:latest
    ports:
      - "5134:5134"
  
  netflix:
    image: netflix:latest
    ports:
      - "5135:5135"
```