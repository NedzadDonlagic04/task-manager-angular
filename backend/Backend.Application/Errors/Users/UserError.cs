using Backend.Shared.Records;

namespace Backend.Application.Errors.Users;

public sealed class UserError
{
    public static readonly Error LoginFailed = new(
        "UserError.LoginFailed",
        "Username or password incorrect",
        ErrorType.Unauthorized
    );
}
