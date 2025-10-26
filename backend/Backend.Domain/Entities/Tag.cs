namespace Backend.Domain.Entities;

public sealed class Tag
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public List<Task> Tasks { get; set; } = [];
}
