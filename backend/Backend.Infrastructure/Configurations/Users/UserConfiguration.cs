using Backend.Domain.Entities.Users;
using Backend.Infrastructure.Abstracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace Backend.Infrastructure.Configurations.Users;

internal sealed class UserConfiguration : EntityTypeConfigurationAndSeeding<UserEntity>
{
    protected override void ConfigureEntity(EntityTypeBuilder<UserEntity> builder)
    {
        builder.HasKey(user => user.Id);
        builder
            .Property(user => user.Id)
            .HasValueGenerator<GuidValueGenerator>()
            .ValueGeneratedOnAdd();

        builder.Property(user => user.Username).IsRequired().HasMaxLength(50);
        builder.HasIndex(user => user.Username).IsUnique();

        builder.Property(user => user.HashedPassword).IsRequired().HasMaxLength(256);

        builder
            .HasOne(user => user.UserProfile)
            .WithOne(userProfile => userProfile.User)
            .HasForeignKey<UserProfileEntity>(userProfile => userProfile.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany(user => user.Tasks)
            .WithOne(task => task.User)
            .HasForeignKey(task => task.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany(user => user.RefreshTokens)
            .WithOne(refreshToken => refreshToken.User)
            .HasForeignKey(refreshToken => refreshToken.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(user => user.CreatedAt).IsRequired().ValueGeneratedOnAdd();
        builder.Property(user => user.UpdatedAt).IsRequired(false).ValueGeneratedOnAddOrUpdate();

        builder.ToTable("User");
    }
}
