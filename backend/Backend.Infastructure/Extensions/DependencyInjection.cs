using Backend.Application.Interfaces;
using Backend.Infastructure.Database;
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
        var connectionString =
            configuration["DATABASE_URL"] ?? throw new Exception("DATABASE_URL not set");

        services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));
        services.AddScoped<IAppDbContext>(scope => scope.GetRequiredService<AppDbContext>());

        return services;
    }
}
