using Backend.Domain.Entities.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Database;

public static class DynamicDataSeeder
{
    public static async Task SeedData(
        AppDbContext context,
        IPasswordHasher<UserEntity> passwordHasher,
        CancellationToken cancellationToken = default
    )
    {
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

        await using var transaction = await context.Database.BeginTransactionAsync(
            cancellationToken
        );

        var stringUser = CreateUserEntity("string", "string", passwordHasher);
        await context.User.AddAsync(stringUser, cancellationToken);

        var stringUserProfile = CreateUserProfileEntity(
            stringUser,
            "String",
            "String",
            "string@example.com"
        );
        await context.UserProfile.AddAsync(stringUserProfile, cancellationToken);

        await context.SaveChangesAsync(cancellationToken);

        await transaction.CommitAsync(cancellationToken);
    }

    private static UserEntity CreateUserEntity(
        string username,
        string password,
        IPasswordHasher<UserEntity> passwordHasher
    )
    {
        var userEntity = new UserEntity { Username = username };

        string mockHashedPassword = passwordHasher.HashPassword(userEntity, password);
        userEntity.HashedPassword = mockHashedPassword;

        return userEntity;
    }

    private static UserProfileEntity CreateUserProfileEntity(
        UserEntity userEntity,
        string firstName,
        string lastName,
        string email
    )
    {
        var stringUserProfile = new UserProfileEntity
        {
            UserId = userEntity.Id,
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            Description = "",
            PictureUrl = null,
            BannerUrl = null,
        };

        return stringUserProfile;
    }
}
