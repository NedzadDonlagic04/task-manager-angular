namespace Backend.Shared.Classes;

public sealed class Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public List<string> Errors { get; } = [];

    private Result()
    {
        IsSuccess = true;
    }

    private Result(List<string> errors)
    {
        IsSuccess = false;
        Errors = errors;
    }

    public static Result Success() => new();

    public static Result Failure(string error) => new([error]);
}

public sealed class Result<T>
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public T? Value { get; }
    public List<string> Errors { get; } = [];

    private Result(T value)
    {
        IsSuccess = true;
        Value = value;
    }

    private Result(List<string> errors)
    {
        IsSuccess = false;
        Errors = errors;
    }

    public static Result<T> Success(T value) => new(value);

    public static Result<T> Failure(string error) => new([error]);

    public static Result<T> Failure(List<string> errors) => new(errors);
}
