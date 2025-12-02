using Backend.Domain.Entities.Users;
using Backend.Domain.Interfaces;

namespace Backend.Domain.Entities.Auth;

public sealed class RefreshTokenEntity : ITimeStampedEntity
{
    public string TokenHash { get; set; } = "";
    public DateTimeOffset ExpiresAt { get; set; }

    public Guid UserId { get; set; }
    public UserEntity User { get; set; } = null!;

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}
