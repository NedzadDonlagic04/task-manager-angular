namespace Backend.API.Extensions;

public static class ApiDependencyInjection
{
    public static IServiceCollection AddAPI(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddControllers();

        var allowedOrigin = configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [];

        services.AddCors(options =>
            options.AddPolicy(
                "AllowFrontend",
                policy => policy.WithOrigins(allowedOrigin).AllowAnyMethod().AllowAnyHeader()
            )
        );

        return services;
    }
}
