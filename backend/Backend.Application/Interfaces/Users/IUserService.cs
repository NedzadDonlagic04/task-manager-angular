using Backend.Application.DTOs.AuthDTO;
using Backend.Shared.Classes;

namespace Backend.Application.Interfaces.Users;

public interface IUserService
{
    Task<Result<LoginResultDTO>> Login(
        LoginCredentialsDTO loginCredentials,
        CancellationToken cancellationToken = default
    );
}
