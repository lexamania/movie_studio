using FluentValidation.Results;

namespace MovieStudio.Domain.Exceptions;

/// <summary>
/// Custom validation exception for CQRS validation failures
/// </summary>
public class ValidationException : Exception
{
    public ValidationException(IEnumerable<ValidationFailure> failures)
        : base("One or more validation failures have occurred.")
    {
        Failures = failures
            .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
            .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
    }

    public IDictionary<string, string[]> Failures { get; }
}
