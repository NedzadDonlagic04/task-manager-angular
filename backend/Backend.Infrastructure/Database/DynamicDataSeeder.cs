using Backend.Domain.Entities.Users;
using Backend.Infrastructure.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Backend.Infrastructure.Database;

public static class DynamicDataSeeder
{
    public static async Task SeedData(
        AppDbContext context,
        IPasswordHasher<UserEntity> passwordHasher,
        CancellationToken cancellationToken = default
    )
    {
        await context.Database.EnsureCreatedAsync(cancellationToken);

        await SeedUserData(context, passwordHasher, cancellationToken);
    }

    private static async Task SeedUserData(
        AppDbContext context,
        IPasswordHasher<UserEntity> passwordHasher,
        CancellationToken cancellationToken = default
    )
    {
        bool doAnyUsersExist = await context.User.AnyAsync(cancellationToken);
        if (doAnyUsersExist)
        {
            return;
        }

        var fixedSeedTime = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero);
        Guid stringUserId = Guid.NewGuid();

        var stringUser = new UserEntity
        {
            Id = stringUserId,
            Username = "string",
            UserProfileId = stringUserId,
            CreatedAt = fixedSeedTime,
            UpdatedAt = null,
        };

        string mockHashedPassword = passwordHasher.HashPassword(stringUser, "string");
        stringUser.HashedPassword = mockHashedPassword;

        await context.User.AddAsync(stringUser, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }
}
