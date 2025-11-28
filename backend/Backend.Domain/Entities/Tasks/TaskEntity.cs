using Backend.Domain.Entities.Users;
using Backend.Domain.Interfaces;

namespace Backend.Domain.Entities.Tasks;

public sealed class TaskEntity : ITimeStampedEntity
{
    public Guid Id { get; set; }
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public DateTimeOffset? Deadline { get; set; }

    public int TaskStateId { get; set; }
    public TaskStateEntity TaskState { get; set; } = null!;

    public List<TagEntity> Tags { get; set; } = [];

    public Guid UserId { get; set; }
    public UserEntity User { get; set; } = null!;

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}
