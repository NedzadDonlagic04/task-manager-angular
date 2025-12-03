using Backend.Application.Interfaces.Auth;
using Backend.Application.Interfaces.Database;
using Backend.Domain.Entities.Users;
using Backend.Infrastructure.Database;
using Backend.Infrastructure.Services.Auth;
using Backend.Shared.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Backend.Infrastructure.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddInfastructure(
        this IServiceCollection services,
        IConfiguration config
    )
    {
        var databaseOptions = config.GetValidatedSection<DatabaseOptions>(
            DatabaseOptions.SectionName
        );

        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(databaseOptions.ConnectionString)
        );
        services.AddScoped<IAppDbContext>(scope => scope.GetRequiredService<AppDbContext>());

        services.AddSingleton<IPasswordHasher<UserEntity>, PasswordHasher<UserEntity>>();
        services.AddSingleton<IJwtService, JwtService>();
        services.AddScoped<IAuthService, AuthService>();

        services
            .AddOptions<RefreshTokenCleanupOptions>()
            .Bind(config.GetSection(RefreshTokenCleanupOptions.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();
        services.AddHostedService<RefreshTokenCleanupService>();

        return services;
    }
}
