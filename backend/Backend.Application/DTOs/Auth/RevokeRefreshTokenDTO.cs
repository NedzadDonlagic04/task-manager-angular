namespace Backend.Application.DTOs.Auth;

public sealed class RevokeRefreshTokenDTO
{
    public required string RefreshToken { get; init; }
}
