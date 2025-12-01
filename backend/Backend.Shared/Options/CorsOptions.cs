using System.ComponentModel.DataAnnotations;

namespace Backend.Shared.Options;

public sealed class CorsOptions
{
    public const string SectionName = "Cors";

    [Required]
    [MinLength(1, ErrorMessage = "Allow origins cannot be an empty array")]
    public required string[] AllowedOrigins { get; init; }

    [Required]
    public required string PolicyName { get; init; }
}
