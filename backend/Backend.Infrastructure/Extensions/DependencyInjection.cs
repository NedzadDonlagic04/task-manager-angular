using Backend.Application.Interfaces;
using Backend.Domain.Entities.Users;
using Backend.Infastructure.Database;
using Backend.Infastructure.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Backend.Infastructure.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddInfastructure(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var databaseOptions = configuration.GetValidatedSection<DatabaseOptions>(
            DatabaseOptions.SectionName
        );

        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(databaseOptions.ConnectionString)
        );
        services.AddScoped<IAppDbContext>(scope => scope.GetRequiredService<AppDbContext>());

        services.AddSingleton<IPasswordHasher<UserEntity>, PasswordHasher<UserEntity>>();

        return services;
    }
}
