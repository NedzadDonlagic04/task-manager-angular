using Backend.Domain.Entities;
using Backend.Infastructure.Abstracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend.Infastructure.Configurations;

internal sealed class UserConfiguration : EntityTypeConfigurationAndSeeding<User>
{
    protected override void ConfigureEntity(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(user => user.Id);

        builder.Property(user => user.Username).IsRequired().HasMaxLength(50);
        builder.HasIndex(user => user.Username).IsUnique();

        builder.Property(user => user.HashedPassword).IsRequired().HasMaxLength(256);

        builder
            .HasOne(user => user.UserProfile)
            .WithOne(userProfile => userProfile.User)
            .HasForeignKey<UserProfile>(up => up.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany(user => user.Tasks)
            .WithOne(task => task.User)
            .HasForeignKey(task => task.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(user => user.CreatedAt).IsRequired().ValueGeneratedOnAdd();
        builder.Property(user => user.UpdatedAt).IsRequired(false).ValueGeneratedOnAddOrUpdate();

        builder.ToTable("User");
    }

    protected override void SeedData(EntityTypeBuilder<User> builder)
    {
        Guid mockUserId = Guid.Parse("9d07ca30-d8f9-40b7-b922-82f567ec6704");
        string mockHashedPassword = "password";
        var fixedSeedTime = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero);

        var mockUser = new User
        {
            Id = mockUserId,
            Username = "mock_user",
            HashedPassword = mockHashedPassword,
            CreatedAt = fixedSeedTime,
            UpdatedAt = null,
        };

        builder.HasData(mockUser);
    }
}
