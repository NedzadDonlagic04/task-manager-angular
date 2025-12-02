namespace Backend.Application.DTOs.AuthDTO;

public sealed class LoginResultDTO
{
    public required string AccessToken { get; init; }
    public required string RefreshToken { get; init; }
    public required DateTimeOffset AccessTokenExpiresAt { get; init; }
}
