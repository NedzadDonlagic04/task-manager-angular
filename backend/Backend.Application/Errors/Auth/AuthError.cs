using Backend.Shared.Records;

namespace Backend.Application.Errors.Auth;

public sealed class AuthError
{
    public static readonly Error LoginFailed = new(
        "AuthError.LoginFailed",
        "Username or password incorrect",
        ErrorType.Unauthorized
    );
}
