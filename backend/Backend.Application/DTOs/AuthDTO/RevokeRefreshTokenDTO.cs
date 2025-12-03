namespace Backend.Application.DTOs.AuthDTO;

public sealed class RevokeRefreshTokenDTO
{
    public required string RefreshToken { get; init; }
}
