namespace Backend.Application.DTOs.AuthDTO;

public sealed class LoginCredentialsDTO
{
    public required string Username { get; init; }

    public required string Password { get; init; }
}
