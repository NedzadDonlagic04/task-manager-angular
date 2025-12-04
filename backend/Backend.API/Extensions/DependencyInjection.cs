using System.Text;
using Backend.Shared.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;

namespace Backend.API.Extensions;

public static class DependencyInjection
{
    private static CorsOptions? s_corsOptions;

    public static IServiceCollection AddAPI(this IServiceCollection services, IConfiguration config)
    {
        s_corsOptions = config.GetValidatedSection<CorsOptions>(CorsOptions.SectionName);

        services
            .AddOptions<JwtOptions>()
            .Bind(config.GetSection(JwtOptions.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

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

            var securityScheme = new OpenApiSecurityScheme
            {
                Name = "JWT Authentication",
                Description = "Enter JWT: ",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer",
                },
            };

            var securityRequirement = new OpenApiSecurityRequirement { { securityScheme, [] } };

            swaggerGenOptions.AddSecurityDefinition("Bearer", securityScheme);
            swaggerGenOptions.AddSecurityRequirement(securityRequirement);
        });

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

        services
            .AddAuthentication(authOptions =>
            {
                authOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                authOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(jwtBearerOptions =>
            {
                var jwtOptions = config.GetValidatedSection<JwtOptions>(JwtOptions.SectionName);

                jwtBearerOptions.TokenValidationParameters = new()
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtOptions.Issuer,
                    ValidateAudience = true,
                    ValidAudience = jwtOptions.Audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtOptions.Key)
                    ),
                };
            });

        services
            .AddAuthorizationBuilder()
            .SetFallbackPolicy(new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build());

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

        app.UseHttpsRedirection();
        app.UseSerilogRequestLogging();

        app.UseCors(s_corsOptions.PolicyName);

        app.UseExceptionHandler();
        app.UseStatusCodePages();

        app.UseAuthentication();
        app.UseAuthorization();

        return app;
    }
}
