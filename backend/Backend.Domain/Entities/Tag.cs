using Backend.Shared.Interfaces;

namespace Backend.Domain.Entities;

public sealed class Tag : ITimeStampedEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = "";

    public List<Task> Tasks { get; set; } = [];

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}
