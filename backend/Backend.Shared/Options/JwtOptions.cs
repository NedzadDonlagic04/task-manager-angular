using System.ComponentModel.DataAnnotations;

namespace Backend.Shared.Options;

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
    public required int AccessTokenMinutes { get; init; }

    [Required]
    public required int RefreshTokenDays { get; init; }
}
