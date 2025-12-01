namespace Backend.Application.DTOs.AuthDTO;

public sealed class LoginResultDTO
{
    public required string AccessToken { get; init; }
}
