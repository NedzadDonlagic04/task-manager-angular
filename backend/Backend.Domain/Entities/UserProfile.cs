using Backend.Shared.Interfaces;

namespace Backend.Domain.Entities;

public sealed class UserProfile : ITimeStampedEntity
{
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string? PictureUrl { get; set; }
    public string? BannerUrl { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}
