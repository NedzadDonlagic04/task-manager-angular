namespace Backend.Application.DTOs.Auth;

public sealed record RevokeRefreshTokenDTO
{
    public required string RefreshToken { get; init; }
}
