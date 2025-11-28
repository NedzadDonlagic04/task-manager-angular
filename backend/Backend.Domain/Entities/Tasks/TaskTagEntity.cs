using Backend.Shared.Interfaces;

namespace Backend.Domain.Entities.Tasks;

public sealed class TaskTagEntity : ITimeStampedEntity
{
    public Guid TaskId { get; set; }
    public TaskEntity Task { get; set; } = null!;

    public Guid TagId { get; set; }
    public TagEntity Tag { get; set; } = null!;

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}
