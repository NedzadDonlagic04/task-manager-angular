using Backend.Domain.Entities.Users;
using Backend.Infastructure.Abstracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend.Infastructure.Configurations;

internal sealed class UserProfileConfiguration
    : EntityTypeConfigurationAndSeeding<UserProfileEntity>
{
    protected override void ConfigureEntity(EntityTypeBuilder<UserProfileEntity> builder)
    {
        builder.HasKey(userProfile => userProfile.UserId);

        builder.Property(userProfile => userProfile.FirstName).IsRequired().HasMaxLength(100);

        builder.Property(userProfile => userProfile.LastName).IsRequired().HasMaxLength(100);

        builder.Property(userProfile => userProfile.Email).IsRequired().HasMaxLength(255);
        builder.HasIndex(userProfile => userProfile.Email).IsUnique();

        builder
            .Property(userProfile => userProfile.Description)
            .IsRequired()
            .HasDefaultValue("")
            .HasMaxLength(500);

        builder.Property(userProfile => userProfile.PictureUrl).IsRequired(false).HasMaxLength(500);

        builder.Property(userProfile => userProfile.BannerUrl).IsRequired(false).HasMaxLength(500);

        builder.Property(userProfile => userProfile.CreatedAt).IsRequired().ValueGeneratedOnAdd();
        builder
            .Property(userProfile => userProfile.UpdatedAt)
            .IsRequired(false)
            .ValueGeneratedOnAddOrUpdate();

        builder.ToTable("UserProfile");
    }

    protected override void SeedData(EntityTypeBuilder<UserProfileEntity> builder)
    {
        var mockUserId = Guid.Parse("9d07ca30-d8f9-40b7-b922-82f567ec6704");
        var fixedSeedTime = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero);

        var mockUserProfile = new UserProfileEntity
        {
            UserId = mockUserId,
            FirstName = "Mock",
            LastName = "Mock",
            Email = "mock@example.com",
            Description = "",
            PictureUrl = null,
            BannerUrl = null,
            CreatedAt = fixedSeedTime,
            UpdatedAt = null,
        };

        builder.HasData(mockUserProfile);
    }
}
