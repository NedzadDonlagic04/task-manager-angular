using Backend.Application.DTOs.AuthDTO;
using Backend.Application.Errors.Users;
using Backend.Application.Interfaces;
using Backend.Application.Interfaces.Auth;
using Backend.Application.Interfaces.Users;
using Backend.Domain.Entities.Users;
using Backend.Shared.Classes;
using Microsoft.EntityFrameworkCore;

namespace Backend.Application.Services.Users;

public sealed class UserService(IAppDbContext context, IJwtService jwtService) : IUserService
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
            return UserError.LoginFailed;
        }

        var accessToken = jwtService.IssueAccessToken(user);

        var loginResponseDTO = new LoginResultDTO { AccessToken = accessToken };

        return Result<LoginResultDTO>.Success(loginResponseDTO);
    }
}
