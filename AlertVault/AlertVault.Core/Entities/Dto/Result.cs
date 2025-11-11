namespace AlertVault.Core.Entities.Dto;

// Create a Result class
public class Result
{
    public bool IsSuccess { get; init; }
    public string? Error { get; init; }
    public List<string> Errors { get; init; }

    protected Result(bool isSuccess, string? error)
    {
        IsSuccess = isSuccess;
        Error = error;
        Errors = [];
    }

    protected Result(bool isSuccess, List<string> errors)
    {
        IsSuccess = isSuccess;
        Errors = errors;
        Error = string.Join(", ", errors);
    }

    public static Result Success() => new(true, (string?)null);
    public static Result Failure(string? error) => new(false, error);
    public static Result Failure(List<string> errors) => new(false, errors);
}

public class Result<T> : Result
{
    public T? Value { get; }

    protected Result(T? value, bool isSuccess, string? error) : base(isSuccess, error)
    {
        Value = value;
    }
    
    protected Result(T? value, bool isSuccess, List<string> errors) : base(isSuccess, errors)
    {
        IsSuccess = isSuccess;
        Errors = errors;
        Error = string.Join(", ", errors);
    }

    public static Result<T?> Success(T? value) => new(value, true, (string?)null);
    public new static Result<T?> Failure(string error) => new(default, false, error);
    public new static Result<T?> Failure(List<string> errors) => new(default, false, errors);
    
    public static implicit operator Result<T?>(T? value) => Success(value);

    public T? Unwrap()
    {
        return !IsSuccess ? throw new Exception(Error ?? "Cannot unwrap a failed Result.") : Value;
    }
}