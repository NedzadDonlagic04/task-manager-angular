using Backend.Application.DTOs.AuthDTO;
using Backend.Shared.Classes;

namespace Backend.Application.Interfaces.Auth;

public interface IAuthService
{
    Task<Result<LoginResultDTO>> Login(
        LoginCredentialsDTO loginCredentials,
        CancellationToken cancellationToken = default
    );

    Task<Result> Logout(
        RevokeRefreshTokenDTO revokeRefreshTokenDTO,
        CancellationToken cancellationToken = default
    );
}
