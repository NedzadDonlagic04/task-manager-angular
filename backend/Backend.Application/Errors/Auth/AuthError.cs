using Backend.Shared.Records;

namespace Backend.Application.Errors.Auth;

public sealed class AuthError
{
    public static readonly Error LoginFailed = new(
        "AuthError.LoginFailed",
        "Username or password incorrect",
        ErrorType.Unauthorized
    );

    public static readonly Error UsernameAlreadyExists = new(
        "AuthError.UsernameAlreadyExists",
        "Username already exists",
        ErrorType.Conflict
    );

    public static readonly Error EmailAlreadyExists = new(
        "AuthError.EmailAlreadyExists",
        "Email already exists",
        ErrorType.Conflict
    );
}
