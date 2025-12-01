using Backend.Domain.Entities.Users;
using Backend.Infastructure.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Backend.Infastructure.Extensions;

public static class DatabaseInitializer
{
    public static async Task InitializeDatabaseAsync(
        this IServiceProvider services,
        IHostEnvironment hostEnv,
        CancellationToken cancellationToken = default
    )
    {
        await using var scope = services.CreateAsyncScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        await context.Database.MigrateAsync(cancellationToken);

        if (hostEnv.IsDevelopment())
        {
            var passwordHasher = services.GetRequiredService<IPasswordHasher<UserEntity>>();

            await DynamicDataSeeder.SeedData(context, passwordHasher, cancellationToken);
        }
    }
}
