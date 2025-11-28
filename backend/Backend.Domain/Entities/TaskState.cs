using Backend.Shared.Interfaces;

namespace Backend.Domain.Entities;

public sealed class TaskState : ITimeStampedEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = "";

    public List<Task> Tasks { get; set; } = [];

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}
