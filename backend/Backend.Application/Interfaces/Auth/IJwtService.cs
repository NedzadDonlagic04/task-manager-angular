using Backend.Domain.Entities.Users;

namespace Backend.Application.Interfaces.Auth;

public sealed class JwtTokens
{
    public required string AccessToken { get; init; }
    public required DateTimeOffset AccessTokenExpiresAt { get; init; }

    public required string RefreshToken { get; init; }
    public required string RefreshTokenHash { get; init; }
    public required DateTimeOffset RefreshTokenExpiresAt { get; init; }
}

public interface IJwtService
{
    JwtTokens IssueAccessToken(UserEntity user);

    string HashRefreshToken(string refreshToken);
}
