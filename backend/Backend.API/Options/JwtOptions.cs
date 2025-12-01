using System.ComponentModel.DataAnnotations;

namespace Backend.API.Options;

public sealed class JwtOptions
{
    public const string SectionName = "Jwt";

    [Required]
    public required string Key { get; init; }

    [Required]
    public required string Issuer { get; init; }

    [Required]
    public required string Audience { get; init; }

    [Required]
    public required string AccessTokenMinutes { get; init; }

    [Required]
    public required string RefreshTokenDays { get; init; }
}
