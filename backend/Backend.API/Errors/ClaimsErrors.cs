using Backend.Shared.Records;

namespace Backend.API.Errors;

public static class ClaimsErrors
{
    public static readonly Error UserIdMissing = new(
        "ClaimsError.UserIdMissing",
        "The authenticated user's ID claim is missing",
        ErrorType.Forbidden
    );
}
