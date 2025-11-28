using Backend.API.Options;
using Microsoft.OpenApi.Models;

namespace Backend.API.Extensions;

public static class DependencyInjection
{
    private static CorsOptions? s_corsOptions;

    public static IServiceCollection AddAPI(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddControllers();

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

        s_corsOptions = configuration.GetValidatedSection<CorsOptions>(CorsOptions.SectionName);

        services.AddCors(options =>
            options.AddPolicy(
                s_corsOptions.PolicyName,
                policy =>
                    policy
                        .WithOrigins(s_corsOptions.AllowedOrigins)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
            )
        );

        services.AddProblemDetails();

        return services;
    }

    public static IApplicationBuilder UseAPI(this IApplicationBuilder app)
    {
        if (s_corsOptions is null)
        {
            throw new InvalidOperationException(
                $"{nameof(AddAPI)} must be called before {nameof(UseAPI)}"
            );
        }

        app.UseCors(s_corsOptions.PolicyName);

        app.UseExceptionHandler();
        app.UseStatusCodePages();

        return app;
    }
}
