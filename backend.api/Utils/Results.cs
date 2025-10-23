namespace Utils;

public class Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public List<string> Errors { get; } = new List<string>();

    private Result()
    {
        IsSuccess = true;
    }

    private Result(List<string> errors)
    {
        IsSuccess = false;
        Errors = errors;
    }

    public static Result Success() => new Result();
    public static Result Failure(string error) => new Result(new List<string> { error });
}

public class Result<T>
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public T? Value { get; }
    public List<string> Errors { get; } = new List<string>();

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

    public static Result<T> Success(T value) => new Result<T>(value);
    public static Result<T> Failure(string error) => new Result<T>(new List<string>() { error });
    public static Result<T> Failure(List<string> errors) => new Result<T>(errors);
}
