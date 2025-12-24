using MediatR;
using Microsoft.Extensions.Logging;

namespace MovieStudio.Shared.Application.Behaviors;

/// <summary>
/// CQRS Logging Behavior - Logs all requests and responses
/// </summary>
public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
        => _logger = logger;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        _logger.LogInformation("Executing {RequestName}", requestName);

        try
        {
            var response = await next.Invoke(cancellationToken);
            _logger.LogInformation("Completed {RequestName}", requestName);
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error executing {RequestName}", requestName);
            throw;
        }
    }
}
