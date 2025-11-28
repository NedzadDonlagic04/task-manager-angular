using Backend.API.Options;
using Microsoft.OpenApi.Models;

namespace Backend.API.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddAPI(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddControllers();

        var corsOptions = configuration.GetValidatedSection<CorsOptions>(CorsOptions.SectionName);

        services.AddCors(options =>
            options.AddPolicy(
                corsOptions.PolicyName,
                policy =>
                    policy.WithOrigins(corsOptions.AllowedOrigins).AllowAnyMethod().AllowAnyHeader()
            )
        );

        services.AddSwaggerGen(swaggerGenOptions =>
        {
            swaggerGenOptions.SwaggerDoc(
                "v1",
                new OpenApiInfo
                {
                    Title = "Task Manager API",
                    Version = "v1",
                    License = new OpenApiLicense
                    {
                        Name = "MIT",
                        Url = new Uri("https://opensource.org/licenses/MIT"),
                    },
                }
            );
        });

        return services;
    }
}
