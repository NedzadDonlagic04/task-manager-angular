using Backend.Shared.Interfaces;

namespace Backend.Domain.Entities.Users;

public sealed class UserProfileEntity : ITimeStampedEntity
{
    public Guid UserId { get; set; }
    public UserEntity User { get; set; } = null!;

    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public string Email { get; set; } = "";
    public string Description { get; set; } = "";
    public string? PictureUrl { get; set; }
    public string? BannerUrl { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}
