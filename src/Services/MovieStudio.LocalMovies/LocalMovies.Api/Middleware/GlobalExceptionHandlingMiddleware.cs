using MovieStudio.Domain.Exceptions;

namespace LocalMovies.Api.Middleware;

/// <summary>
/// Global exception handling middleware for consistent error responses
/// </summary>
public class GlobalExceptionHandlingMiddleware(
    RequestDelegate next, 
    ILogger<GlobalExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An unhandled exception occurred");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var response = new ErrorResponse
        {
            Message = exception.Message,
            Timestamp = DateTime.UtcNow
        };

        switch (exception)
        {
            case ValidationException validationEx:
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                response.Errors = validationEx.Failures;
                break;
            case KeyNotFoundException:
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                break;
            case ArgumentException:
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                break;
            default:
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                response.Message = "An internal server error occurred";
                break;
        }

        return context.Response.WriteAsJsonAsync(response);
    }

    private class ErrorResponse
    {
        public string Message { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public IDictionary<string, string[]>? Errors { get; set; }
    }
}
