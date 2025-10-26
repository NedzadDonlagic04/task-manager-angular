namespace Backend.Domain.Models;

public sealed class Task
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime? Deadline { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public int TaskStateId { get; set; }
    public TaskState TaskState { get; set; } = null!;
    public List<Tag> Tags { get; set; } = [];
    public User User { get; set; } = null!;
}
