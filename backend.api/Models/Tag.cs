namespace Models;

public sealed class Tag
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = null!;
    public List<Task> Tasks { get; set; } = new();
}