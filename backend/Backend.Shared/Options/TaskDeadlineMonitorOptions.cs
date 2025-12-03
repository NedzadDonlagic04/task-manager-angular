using System.ComponentModel.DataAnnotations;

namespace Backend.Shared.Options;

public sealed class TaskDeadlineMonitorOptions
{
    public const string SectionName = "BackgroundServices:TaskDeadlineMonitor";

    [Required]
    public required bool RunOnStart { get; init; }

    [Required]
    public required int CheckExpiredDeadlineSeconds { get; init; }
}
