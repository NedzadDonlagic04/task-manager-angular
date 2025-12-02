using Backend.Domain.Entities.Auth;
using Backend.Domain.Entities.Tasks;
using Backend.Domain.Interfaces;

namespace Backend.Domain.Entities.Users;

public sealed class UserEntity : ITimeStampedEntity
{
    public Guid Id { get; set; }
    public string Username { get; set; } = "";
    public string HashedPassword { get; set; } = "";

    public Guid UserProfileId { get; set; }
    public UserProfileEntity UserProfile { get; set; } = null!;

    public List<TaskEntity> Tasks { get; set; } = [];

    public List<RefreshTokenEntity> RefreshTokens { get; set; } = [];

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}
