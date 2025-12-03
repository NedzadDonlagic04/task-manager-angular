using Backend.Domain.Entities.Users;
using Backend.Infrastructure.Abstracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend.Infrastructure.Configurations.Users;

internal sealed class UserProfileConfiguration
    : EntityTypeConfigurationAndSeeding<UserProfileEntity>
{
    protected override void ConfigureEntity(EntityTypeBuilder<UserProfileEntity> builder)
    {
        builder.HasKey(userProfile => userProfile.UserId);
        builder.Property(userProfile => userProfile.UserId).ValueGeneratedNever();

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
}
