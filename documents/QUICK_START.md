# Quick Start Guide - Refactored MovieStudio Architecture

## Project Structure Overview

```
src/
├── Shared/
│   ├── MovieStudio.Application/     # CQRS Application Layer
│   │   ├── Behaviors/               # MediatR pipeline behaviors
│   │   ├── Commands/                # CQRS Commands
│   │   ├── Queries/                 # CQRS Queries
│   │   ├── Validators/              # FluentValidation rules
│   │   ├── DTOs/                    # Data transfer objects
│   │   └── Handlers/                # Handler interfaces
│   │
│   └── MovieStudio.Domain/          # Domain Layer
│       ├── Models/                  # Domain entities
│       ├── Events/                  # Domain events
│       ├── Exceptions/              # Custom exceptions
│       ├── Results/                 # Result wrapper
│       └── Interfaces/              # Domain contracts
│
└── Services/
    └── MovieStudio.LocalMovies/
        ├── LocalMovies.Api/         # API Layer & Endpoints
        │   ├── Endpoints/           # HTTP endpoint mappings
        │   ├── Middleware/          # Exception handling
        │   ├── Validators/          # Command validators
        │   ├── EventHandlers/       # Domain event handlers
        │   └── Configurations/      # DI setup
        │
        └── LocalMovies.Infrastructure/  # Infrastructure Layer
            ├── Services/            # Business logic
            ├── Parsers/             # File parsers (CSV, etc.)
            ├── DirectoryData/       # Data models & events
            ├── Interfaces/          # Contracts
            └── Configurations/      # Infrastructure setup
```

---

## Key Concepts

### 1. Commands (State Mutations)
**Purpose:** Change the state of your system

**Example:** Adding a new movie directory
```csharp
// Define
public record AddDirectoryCommand(string DirectoryPath, string Caption, bool IncludeInner) : IRequest<bool>;

// Validate
public class AddDirectoryCommandValidator : AbstractValidator<AddDirectoryCommand>
{
    public AddDirectoryCommandValidator()
    {
        RuleFor(x => x.DirectoryPath).NotEmpty().Must(DirectoryExists);
    }
}

// Handle
public class AddDirectoryHandler : IRequestHandler<AddDirectoryCommand, bool>
{
    public async Task<bool> Handle(AddDirectoryCommand request, CancellationToken ct)
    {
        storage.AddDirectory(request.DirectoryPath, request.Caption, request.IncludeInner);
        
        // Publish event for other services
        await mediator.Publish(new DirectoryAddedEvent { ... });
        
        return true;
    }
}

// Expose via Endpoint
app.MapPost("/directories", async (NewDirectoryModel model, IMediator mediator) =>
{
    var result = await mediator.Send(new AddDirectoryCommand(...));
    return Results.Created();
});
```

---

### 2. Queries (Read-Only Operations)
**Purpose:** Retrieve data without changing state

**Example:** Getting all movies
```csharp
// Define
public record GetAllMoviesQuery(PaginationModel Pagination) : IRequest<List<MovieShortDto>>;

// Validate
public class GetAllMoviesQueryValidator : AbstractValidator<GetAllMoviesQuery>
{
    public GetAllMoviesQueryValidator()
    {
        RuleFor(x => x.Pagination.PageSize).InclusiveBetween(1, 100);
    }
}

// Handle
public class GetAllMoviesHandler : IGetAllMoviesHandler
{
    public async Task<List<MovieShortDto>> Handle(GetAllMoviesQuery request, CancellationToken ct)
    {
        var movies = storage.GetDirectories()
            .SelectMany(d => d.Files)
            .WithPagination(request.Pagination)
            .ToList();
        
        return movies;
    }
}

// Expose via Endpoint
app.MapGet("/movies/all", async (PaginationModel pagination, IMediator mediator) =>
{
    var result = await mediator.Send(new GetAllMoviesQuery(pagination));
    return Results.Ok(result);
});
```

---

### 3. Domain Events (Async Communication)
**Purpose:** Notify other services about important state changes

**Example:** Directory added event
```csharp
// Define Event
public class DirectoryAddedEvent : DomainEvent
{
    public string DirectoryId { get; set; }
    public string DirectoryPath { get; set; }
    public string Caption { get; set; }
}

// Publish in Handler
await mediator.Publish(new DirectoryAddedEvent { ... });

// Handle Event (in any service)
public class DirectoryEventHandler : INotificationHandler<DirectoryAddedEvent>
{
    public async Task Handle(DirectoryAddedEvent evt, CancellationToken ct)
    {
        // Send to RabbitMQ for other services
        // Update cache
        // Log to analytics
        // Send notification to user
    }
}
```

---

### 4. Validation Pipeline
**Purpose:** Automatically validate all requests before they reach handlers

**Flow:**
```
Request → ValidationBehavior → Validate with all validators → Handler
                                       ↓
                              If invalid: throw ValidationException
```

**Example:**
```csharp
// Invalid request
POST /directories
{
    "directoryPath": "/invalid/path",
    "caption": "",
    "includeInner": true
}

// Response: 400 Bad Request
{
    "message": "One or more validation failures have occurred.",
    "timestamp": "2025-12-23T10:30:00Z",
    "errors": {
        "directoryPath": ["Directory path does not exist"],
        "caption": ["Caption is required"]
    }
}
```

---

### 5. Error Handling Middleware
**Purpose:** Catch all exceptions and return consistent error responses

**Example:**
```csharp
// In GlobalExceptionHandlingMiddleware
public async Task InvokeAsync(HttpContext context)
{
    try
    {
        await _next(context);
    }
    catch (ValidationException ex)
    {
        context.Response.StatusCode = 400;
        await context.Response.WriteAsJsonAsync(new { message = ex.Message, errors = ex.Failures });
    }
    catch (Exception ex)
    {
        context.Response.StatusCode = 500;
        await context.Response.WriteAsJsonAsync(new { message = "Internal server error" });
    }
}
```

---

### 6. Logging Pipeline
**Purpose:** Automatically log all requests and responses

**Output:**
```
info: LoggingBehavior[0]
      Executing AddDirectoryCommand
info: LoggingBehavior[0]
      Completed AddDirectoryCommand
```

---

## Common Tasks

### Add a New Command

1. **Create the command:**
```csharp
// src/Shared/MovieStudio.Application/Commands/MyNewCommand.cs
public record MyNewCommand(string Property) : IRequest<bool>;
```

2. **Create a validator:**
```csharp
// src/Services/MovieStudio.LocalMovies/LocalMovies.Api/Application/Validators/CommandValidators.cs
public class MyNewCommandValidator : AbstractValidator<MyNewCommand>
{
    public MyNewCommandValidator()
    {
        RuleFor(x => x.Property).NotEmpty();
    }
}
```

3. **Create a handler:**
```csharp
// src/Services/MovieStudio.LocalMovies/LocalMovies.Api/Application/Commands/MyNewHandler.cs
public class MyNewHandler(StorageService storage, IMediator mediator) : IRequestHandler<MyNewCommand, bool>
{
    public async Task<bool> Handle(MyNewCommand request, CancellationToken ct)
    {
        // Do something
        await mediator.Publish(new MyNewEvent { ... });
        return true;
    }
}
```

4. **Register validator in Program.cs:**
```csharp
builder.Services.AddLocalMoviesServices(); // Already handles this
```

5. **Create an endpoint:**
```csharp
// In ConfigureEndpoints.cs or new file
app.MapPost("/my-resource", PostMyResource);

public static async Task<IResult> PostMyResource(
    [FromBody] MyRequest request,
    IMediator mediator)
{
    var result = await mediator.Send(new MyNewCommand(request.Property));
    return Results.Created();
}
```

---

### Add a New Query

1. **Create the query:**
```csharp
public record GetMyDataQuery(int Id) : IRequest<MyDataDto>;
```

2. **Create a validator:**
```csharp
public class GetMyDataQueryValidator : AbstractValidator<GetMyDataQuery>
{
    public GetMyDataQueryValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
    }
}
```

3. **Create a handler:**
```csharp
public class GetMyDataHandler : IRequestHandler<GetMyDataQuery, MyDataDto>
{
    public async Task<MyDataDto> Handle(GetMyDataQuery request, CancellationToken ct)
    {
        // Fetch data
        return new MyDataDto { ... };
    }
}
```

4. **Register validator in Program.cs:**
```csharp
builder.Services.AddLocalMoviesServices();
```

5. **Create an endpoint:**
```csharp
app.MapGet("/my-resource/{id}", GetMyResource);

public static async Task<IResult> GetMyResource(
    [FromRoute] int id,
    IMediator mediator)
{
    var result = await mediator.Send(new GetMyDataQuery(id));
    return Results.Ok(result);
}
```

---

### Add a Domain Event

1. **Create the event:**
```csharp
// src/Shared/MovieStudio.Domain/Events/MyEvent.cs
public class MyEvent : DomainEvent
{
    public string MyProperty { get; set; }
}
```

2. **Publish in handler:**
```csharp
public async Task<bool> Handle(MyCommand request, CancellationToken ct)
{
    // Do something
    await mediator.Publish(new MyEvent { MyProperty = "value" });
    return true;
}
```

3. **Create an event handler:**
```csharp
public class MyEventHandler : INotificationHandler<MyEvent>
{
    private readonly ILogger<MyEventHandler> _logger;
    
    public async Task Handle(MyEvent notification, CancellationToken ct)
    {
        _logger.LogInformation("MyEvent fired!");
        // Do something async
    }
}
```

---

## Testing

### Unit Test a Handler
```csharp
[TestFixture]
public class AddDirectoryHandlerTests
{
    [Test]
    public async Task Handle_ValidCommand_AddDirectory()
    {
        // Arrange
        var storageMock = new Mock<StorageService>();
        var handler = new AddDirectoryHandler(storageMock.Object);
        var command = new AddDirectoryCommand("/path", "Caption", true);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.IsTrue(result);
        storageMock.Verify(s => s.AddDirectory(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()), Times.Once);
    }
}
```

### Integration Test an Endpoint
```csharp
[TestFixture]
public class DirectoryEndpointTests
{
    private WebApplicationFactory<Program> _factory;

    [SetUp]
    public void SetUp()
    {
        _factory = new WebApplicationFactory<Program>();
    }

    [Test]
    public async Task PostDirectory_InvalidPath_Returns400()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.PostAsJsonAsync(
            "/directories",
            new { directoryPath = "/invalid", caption = "", includeInner = true });

        // Assert
        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
```

---

## Debugging

### Enable Detailed Logging
```csharp
// In Program.cs
builder.Logging.SetMinimumLevel(LogLevel.Debug);
builder.Logging.AddConsole();
```

### Log Request/Response
Already done via `LoggingBehavior`! You'll see:
```
info: MovieStudio.Application.Behaviors.LoggingBehavior[0]
      Executing AddDirectoryCommand
info: MovieStudio.Application.Behaviors.LoggingBehavior[0]
      Completed AddDirectoryCommand
```

### Inspect Validation Errors
Add breakpoint in `ValidationBehavior`:
```csharp
var failures = validationResults
    .Where(r => r.Errors.Any())
    .SelectMany(r => r.Errors)
    .ToList();
```

---

## Performance Tips

1. **Cache Query Results:**
```csharp
services.AddStackExchangeRedisCache(options => 
    options.Configuration = redisConnectionString);
```

2. **Add Pagination:**
```csharp
RuleFor(x => x.Pagination.PageSize)
    .InclusiveBetween(1, PaginationModel.MAX_PAGE_SIZE);
```

3. **Use Async/Await:**
All handlers are async by default with MediatR.

4. **Index Frequently Queried Data:**
If using database, add indexes on frequently filtered columns.

---

## References

- **ARCHITECTURE.md** - Complete architecture guide
- **REFACTORING_SUMMARY.md** - Detailed refactoring changes
- [MediatR Documentation](https://github.com/jbogard/MediatR)
- [FluentValidation Documentation](https://docs.fluentvalidation.net/)
- [CQRS Pattern](https://www.microsoft.com/en-us/research/publication/cqrs-pattern/)
- [Domain-Driven Design](https://en.wikipedia.org/wiki/Domain-driven_design)

---

## Next Steps

1. ✅ Refactoring complete
2. Build and test the solution
3. Add more services (HBO, Netflix)
4. Set up message queue for inter-service communication
5. Replace CSV with proper database
6. Add comprehensive tests
7. Deploy microservices independently

**Happy coding! 🚀**
