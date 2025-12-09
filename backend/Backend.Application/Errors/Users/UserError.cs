using Backend.Shared.Records;

namespace Backend.Application.Errors.Users;

public static class UserError
{
    public static readonly Error NotFound = new(
        "UserError.NotFound",
        "User not found",
        ErrorType.NotFound
    );
}
