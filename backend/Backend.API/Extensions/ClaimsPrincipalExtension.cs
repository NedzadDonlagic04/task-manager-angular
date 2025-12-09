using System.Security.Claims;

namespace Backend.API.Extensions;

public static class ClaimsPrincipalExtension
{
    public static Guid GetUserId(this ClaimsPrincipal claimsPrincipal)
    {
        string userId =
            claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? throw new UnauthorizedAccessException("Missing NameIdentifier claim");

        return Guid.Parse(userId);
    }
}
