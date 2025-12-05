namespace Backend.Application.DTOs.Auth;

public sealed record LoginCredentialsDTO
{
    public required string Username { get; init; }

    public required string Password { get; init; }
}
