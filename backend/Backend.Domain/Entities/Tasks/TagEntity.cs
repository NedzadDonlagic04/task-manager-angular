using Backend.Domain.Interfaces;

namespace Backend.Domain.Entities.Tasks;

public sealed class TagEntity : ITimeStampedEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = "";

    public List<TaskEntity> Tasks { get; set; } = [];

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}
