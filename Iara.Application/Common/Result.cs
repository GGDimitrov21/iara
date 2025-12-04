namespace Iara.Application.Common;

/// <summary>
/// Generic result wrapper for operation outcomes
/// </summary>
/// <typeparam name="T">Result data type</typeparam>
public class Result<T>
{
    public bool IsSuccess { get; }
    public T? Data { get; }
    public string? ErrorMessage { get; }
    public Dictionary<string, string[]>? ValidationErrors { get; }

    private Result(bool isSuccess, T? data, string? errorMessage, Dictionary<string, string[]>? validationErrors = null)
    {
        IsSuccess = isSuccess;
        Data = data;
        ErrorMessage = errorMessage;
        ValidationErrors = validationErrors;
    }

    public static Result<T> Success(T data) => new(true, data, null);
    public static Result<T> Failure(string errorMessage) => new(false, default, errorMessage);
    public static Result<T> ValidationFailure(Dictionary<string, string[]> validationErrors) 
        => new(false, default, "Validation failed", validationErrors);
}

/// <summary>
/// Non-generic result for operations without return data
/// </summary>
public class Result
{
    public bool IsSuccess { get; }
    public string? ErrorMessage { get; }

    private Result(bool isSuccess, string? errorMessage)
    {
        IsSuccess = isSuccess;
        ErrorMessage = errorMessage;
    }

    public static Result Success() => new(true, null);
    public static Result Failure(string errorMessage) => new(false, errorMessage);
}
