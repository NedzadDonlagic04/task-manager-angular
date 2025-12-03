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

    Task<Result> Register(
        RegisterAccountDTO registerAccountDTO,
        CancellationToken cancellationToken = default
    );
}
