using System.ComponentModel.DataAnnotations;

namespace Backend.Shared.Options;

public sealed class DatabaseOptions
{
    public const string SectionName = "Database";

    [Required]
    public required string ConnectionString { get; init; }

    [Required]
    public required bool EnableSensitiveDataLogging { get; init; }

    [Required]
    public required bool EnableDetailedErrors { get; init; }
}
