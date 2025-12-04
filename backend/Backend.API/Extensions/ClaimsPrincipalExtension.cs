using System.Security.Claims;
using Backend.API.Errors;
using Backend.Shared.Classes;

namespace Backend.API.Extensions;

public static class ClaimsPrincipalExtension
{
    public static Result<Guid> GetUserId(this ClaimsPrincipal claimsPrincipal)
    {
        string? userId = claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);

        return userId is null
            ? ClaimsErrors.UserIdMissing
            : Result<Guid>.Success(Guid.Parse(userId));
    }
}
