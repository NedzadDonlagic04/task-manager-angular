using Backend.Application.DTOs.AuthDTO;
using Backend.Application.Errors.Auth;
using Backend.Application.Interfaces;
using Backend.Application.Interfaces.Auth;
using Backend.Domain.Entities.Auth;
using Backend.Domain.Entities.Users;
using Backend.Shared.Classes;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Services.Auth;

public sealed class AuthService(
    IAppDbContext context,
    IJwtService jwtService,
    IPasswordHasher<UserEntity> passwordHasher
) : IAuthService
{
    public async Task<Result<LoginResultDTO>> Login(
        LoginCredentialsDTO loginCredentials,
        CancellationToken cancellationToken = default
    )
    {
        var user = await context
            .Set<UserEntity>()
            .FirstOrDefaultAsync(
                user => user.Username == loginCredentials.Username,
                cancellationToken
            );

        if (user is null)
        {
            return AuthError.LoginFailed;
        }

        var passwordVerificationResult = passwordHasher.VerifyHashedPassword(
            user,
            user.HashedPassword,
            loginCredentials.Password
        );

        if (passwordVerificationResult == PasswordVerificationResult.Failed)
        {
            return AuthError.LoginFailed;
        }

        var jwtTokens = jwtService.IssueAccessToken(user);

        var refreshTokenEntity = new RefreshTokenEntity
        {
            TokenHash = jwtTokens.RefreshTokenHash,
            ExpiresAt = jwtTokens.RefreshTokenExpiresAt,
            UserId = user.Id,
        };

        await context.Set<RefreshTokenEntity>().AddAsync(refreshTokenEntity, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        var loginResponseDTO = new LoginResultDTO
        {
            AccessToken = jwtTokens.AccessToken,
            RefreshToken = jwtTokens.RefreshToken,
            AccessTokenExpiresAt = jwtTokens.AccessTokenExpiresAt,
        };

        return Result<LoginResultDTO>.Success(loginResponseDTO);
    }

    public async Task<Result> Logout(
        RevokeRefreshTokenDTO revokeRefreshTokenDTO,
        CancellationToken cancellationToken = default
    )
    {
        var refreshTokenHash = jwtService.HashRefreshToken(revokeRefreshTokenDTO.RefreshToken);
        var refreshTokenEntity = await context
            .Set<RefreshTokenEntity>()
            .FirstOrDefaultAsync(
                refreshToken => refreshToken.TokenHash == refreshTokenHash,
                cancellationToken
            );

        if (refreshTokenEntity is not null)
        {
            context.Set<RefreshTokenEntity>().Remove(refreshTokenEntity);
            await context.SaveChangesAsync(cancellationToken);
        }

        return Result.Success();
    }
}
