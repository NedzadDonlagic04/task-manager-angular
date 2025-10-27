using Backend.Shared.Interfaces;

namespace Backend.Domain.Entities;

public sealed class TaskTag : ITimeStampedEntity
{
    public Guid TaskId { get; set; }
    public Task Task { get; set; } = null!;

    public Guid TagId { get; set; }
    public Tag Tag { get; set; } = null!;

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}
