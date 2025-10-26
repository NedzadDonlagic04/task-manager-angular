using System.ComponentModel.DataAnnotations;

namespace Backend.Domain.Models;

public sealed class UserProfile
{
    [Key]
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string? PictureUrl { get; set; }
    public string? BannerUrl { get; set; }
}
