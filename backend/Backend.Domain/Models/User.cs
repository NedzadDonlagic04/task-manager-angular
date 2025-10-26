namespace Backend.Domain.Models;

public sealed class User
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Username { get; set; } = null!;
    public string HashedPassword { get; set; } = null!;
    public UserProfile UserProfile { get; set; } = null!;
    public List<Task> Tasks { get; set; } = [];
}
