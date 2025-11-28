using Backend.Shared.Interfaces;

namespace Backend.Domain.Entities;

public sealed class User : ITimeStampedEntity
{
    public Guid Id { get; set; }
    public string Username { get; set; } = "";
    public string HashedPassword { get; set; } = "";

    public Guid UserProfileId { get; set; }
    public UserProfile UserProfile { get; set; } = null!;

    public List<Task> Tasks { get; set; } = [];

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}
