namespace Backend.Domain.Entities;

public sealed class User
{
    public Guid Id { get; set; }
    public string Username { get; set; } = null!;
    public string HashedPassword { get; set; } = null!;
    public UserProfile UserProfile { get; set; } = null!;
    public List<Task> Tasks { get; set; } = [];
}
