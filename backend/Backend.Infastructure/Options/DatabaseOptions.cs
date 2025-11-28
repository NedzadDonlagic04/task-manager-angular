using System.ComponentModel.DataAnnotations;

namespace Backend.Infastructure.Options;

public sealed class DatabaseOptions
{
    public const string SectionName = "Database";

    [Required]
    public required string ConnectionString { get; init; }
}
