using Backend.Shared.Interfaces;

namespace Backend.Domain.Entities;

public sealed class Task : ITimeStampedEntity
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTimeOffset? Deadline { get; set; }

    public int TaskStateId { get; set; }
    public TaskState TaskState { get; set; } = null!;

    public List<Tag> Tags { get; set; } = [];

    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}
