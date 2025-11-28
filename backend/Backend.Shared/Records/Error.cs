namespace Backend.Shared.Records;

public sealed record Error(string Code, string Detail, ErrorType type)
{
    public static readonly Error None = new("", "", ErrorType.None);

    public static readonly Error NullValue = new(
        "Error.NullValue",
        "A null value was provided",
        ErrorType.Validation
    );
}

public enum ErrorType
{
    None,
    Unexpected,
    Validation,
    NotFound,
    Conflict,
    Forbidden,
    Unauthorized,
}
