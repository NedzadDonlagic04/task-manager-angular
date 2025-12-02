using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Backend.Application.Interfaces.Auth;
using Backend.Domain.Entities.Users;
using Backend.Shared.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Backend.Infrastructure.Services.Auth;

public sealed class JwtService(IOptions<JwtOptions> options) : IJwtService
{
    private readonly JwtOptions _jwtOptions = options.Value;

    public JwtTokens IssueAccessToken(UserEntity user)
    {
        ArgumentNullException.ThrowIfNull(user, nameof(user));

        var now = DateTimeOffset.UtcNow;
        var expiresAt = now.AddMinutes(_jwtOptions.AccessTokenMinutes).UtcDateTime;

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Iat, now.ToUnixTimeSeconds().ToString()),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
        };

        var signinKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key));
        var credentials = new SigningCredentials(signinKey, SecurityAlgorithms.HmacSha256);

        var jwt = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            claims: claims,
            signingCredentials: credentials,
            expires: expiresAt,
            notBefore: now.UtcDateTime
        );

        var accessToken = new JwtSecurityTokenHandler().WriteToken(jwt);
        string refreshToken = GenerateRefreshToken();
        string refreshTokenHash = HashRefreshToken(refreshToken);
        var refreshTokenExpiresAt = now.AddDays(_jwtOptions.RefreshTokenDays);

        var jwtTokens = new JwtTokens
        {
            AccessToken = accessToken,
            AccessTokenExpiresAt = expiresAt,
            RefreshToken = refreshToken,
            RefreshTokenHash = refreshTokenHash,
            RefreshTokenExpiresAt = refreshTokenExpiresAt,
        };

        return jwtTokens;
    }

    public string GenerateRefreshToken(int numberOfBytes = 64)
    {
        var bytes = RandomNumberGenerator.GetBytes(numberOfBytes);

        return Convert.ToBase64String(bytes);
    }

    public string HashRefreshToken(string refreshToken)
    {
        ArgumentNullException.ThrowIfNullOrEmpty(refreshToken, nameof(refreshToken));

        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(refreshToken));

        return Convert.ToBase64String(bytes);
    }
}
