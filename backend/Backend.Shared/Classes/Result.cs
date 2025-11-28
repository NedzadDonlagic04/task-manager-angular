using Backend.Shared.Records;

namespace Backend.Shared.Classes;

public class Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public List<Error> Errors { get; } = [];

    public static Result Success() => new();

    public static Result Failure(Error error) => new(error);

    public static Result Failure(List<Error> errors) => new(errors);

    public static implicit operator Result(Error error) => new(error);

    public static implicit operator Result(List<Error> errors) => new(errors);

    protected Result()
    {
        IsSuccess = true;
    }

    protected Result(Error error)
    {
        ArgumentNullException.ThrowIfNull(error, nameof(error));

        IsSuccess = false;
        Errors = [error];
    }

    protected Result(List<Error> errors)
    {
        ArgumentNullException.ThrowIfNull(errors, nameof(errors));
        ArgumentOutOfRangeException.ThrowIfZero(errors.Count, nameof(errors));

        IsSuccess = false;
        Errors = errors;
    }
}

public sealed class Result<T> : Result
{
    private readonly T? _value;

    public T Value =>
        IsSuccess
            ? _value!
            : throw new InvalidOperationException(
                $"Cannot access Value of a failed result. Errors: {string.Join(", ", Errors.Select(error => error.Code))}"
            );

    public static Result<T> Success(T value) => new(value);

    public static new Result<T> Failure(Error error) => new(error);

    public static new Result<T> Failure(List<Error> errors) => new(errors);

    public static implicit operator Result<T>(Error error) => new(error);

    public static implicit operator Result<T>(List<Error> errors) => new(errors);

    private Result(T value)
        : base()
    {
        ArgumentNullException.ThrowIfNull(value, nameof(value));

        _value = value;
    }

    private Result(Error error)
        : base(error) { }

    private Result(List<Error> errors)
        : base(errors) { }
}
