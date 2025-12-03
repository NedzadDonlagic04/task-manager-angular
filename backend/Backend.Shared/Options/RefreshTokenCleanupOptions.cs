using System.ComponentModel.DataAnnotations;

namespace Backend.Shared.Options;

public sealed class RefreshTokenCleanupOptions
{
    public const string SectionName = "BackgroundServices:RefreshTokenCleanup";

    [Required]
    public required bool RunOnStart { get; init; }

    [Required]
    public required int CleaupHours { get; init; }
}
