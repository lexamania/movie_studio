namespace MovieStudio.Domain.Results;

/// <summary>
/// Generic result wrapper for CQRS responses
/// Enables consistent error and success handling across all operations
/// </summary>
public class Result<T> : IResult
{
    public bool IsSuccess { get; private set; }
    public T? Data { get; private set; }
    public string? Error { get; private set; }

    public static Result<T> Success(T data)
        => new() { IsSuccess = true, Data = data };

    public static Result<T> Failure(string error)
        => new() { IsSuccess = false, Error = error };
}

public class Result : IResult
{
    public bool IsSuccess { get; private set; }
    public string? Error { get; private set; }

    public static Result Success()
        => new() { IsSuccess = true };

    public static Result Failure(string error)
        => new() { IsSuccess = false, Error = error };
}

public interface IResult
{
    bool IsSuccess { get; }
    string? Error { get; }
}
