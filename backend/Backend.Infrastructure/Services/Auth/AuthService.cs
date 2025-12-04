using Backend.Application.DTOs.Auth;
using Backend.Application.Errors.Auth;
using Backend.Application.Interfaces.Auth;
using Backend.Application.Interfaces.Database;
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
        var loginValidationResult = await ValidateLoginCredentials(
            loginCredentials,
            cancellationToken
        );

        if (loginValidationResult.IsFailure)
        {
            return loginValidationResult.Errors;
        }

        var user = loginValidationResult.Value;

        var loginResponseDTO = await IssueTokens(user, cancellationToken);

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

    public async Task<Result> Register(
        RegisterAccountDTO registerAccountDTO,
        CancellationToken cancellationToken = default
    )
    {
        var registrationValidationResult = await ValidateRegistrationRequest(
            registerAccountDTO,
            cancellationToken
        );

        if (registrationValidationResult.IsFailure)
        {
            return registrationValidationResult.Errors;
        }

        await CreateUserWithProfile(registerAccountDTO, cancellationToken);

        return Result.Success();
    }

    private async Task<Result<UserEntity>> ValidateLoginCredentials(
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

        return Result<UserEntity>.Success(user);
    }

    private async Task<LoginResultDTO> IssueTokens(
        UserEntity user,
        CancellationToken cancellationToken = default
    )
    {
        var jwtTokens = jwtService.IssueAccessToken(user);

        var refreshTokenEntity = new RefreshTokenEntity
        {
            TokenHash = jwtTokens.RefreshTokenHash,
            ExpiresAt = jwtTokens.RefreshTokenExpiresAt,
            UserId = user.Id,
        };

        await context.Set<RefreshTokenEntity>().AddAsync(refreshTokenEntity, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        var loginResultDTO = new LoginResultDTO
        {
            AccessToken = jwtTokens.AccessToken,
            RefreshToken = jwtTokens.RefreshToken,
            AccessTokenExpiresAt = jwtTokens.AccessTokenExpiresAt,
        };

        return loginResultDTO;
    }

    private async Task<Result> ValidateRegistrationRequest(
        RegisterAccountDTO registerAccountDTO,
        CancellationToken cancellationToken = default
    )
    {
        bool isUsernameTaken = await context
            .Set<UserEntity>()
            .Where(user => user.Username == registerAccountDTO.Username)
            .AnyAsync(cancellationToken);

        if (isUsernameTaken)
        {
            return AuthError.UsernameAlreadyExists;
        }

        bool isEmailTaken = await context
            .Set<UserProfileEntity>()
            .Where(userProfile => userProfile.Email == registerAccountDTO.Email)
            .AnyAsync(cancellationToken);

        if (isEmailTaken)
        {
            return AuthError.EmailAlreadyExists;
        }

        return Result.Success();
    }

    private async Task CreateUserWithProfile(
        RegisterAccountDTO registerAccountDTO,
        CancellationToken cancellationToken = default
    )
    {
        await using var transaction = await context.BeginTransactionAsync(cancellationToken);

        var user = new UserEntity { Username = registerAccountDTO.Username };
        user.HashedPassword = passwordHasher.HashPassword(user, registerAccountDTO.Password);

        await context.Set<UserEntity>().AddAsync(user, cancellationToken);

        var userProfile = new UserProfileEntity
        {
            UserId = user.Id,
            FirstName = registerAccountDTO.FirstName,
            LastName = registerAccountDTO.LastName,
            Email = registerAccountDTO.Email,
        };

        await context.Set<UserProfileEntity>().AddAsync(userProfile, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        await transaction.CommitAsync(cancellationToken);
    }
}
