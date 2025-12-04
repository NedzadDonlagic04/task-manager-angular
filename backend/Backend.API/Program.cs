using Backend.API.Extensions;
using Backend.Application.Extensions;
using Backend.Infrastructure.Extensions;
using Serilog;

Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateBootstrapLogger();

var startupLogger = Log.ForContext<Program>();

try
{
    startupLogger.Information("Starting Task Manager API...");

    var builder = WebApplication.CreateBuilder(args);

    builder.Logging.ClearProviders();
    builder.Host.UseSerilog(
        (context, services, config) =>
            config.ReadFrom.Configuration(context.Configuration).ReadFrom.Services(services)
    );

    startupLogger.Information(
        "Running in environment: {EnvironmentName}",
        builder.Environment.EnvironmentName
    );

    startupLogger.Information(
        "Registering dependency injection modules (API, Infrastructure, Application)..."
    );
    builder
        .Services.AddAPI(builder.Configuration)
        .AddInfastructure(builder.Configuration)
        .AddApplication(builder.Configuration);

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        startupLogger.Information(
            "Development environment detected, enabling swagger and swagger UI..."
        );
        app.MapSwagger().AllowAnonymous();
        app.UseSwaggerUI();
    }

    startupLogger.Information("Running database initialization and migrations...");
    await app.Services.InitializeDatabaseAsync(app.Environment);
    startupLogger.Information("Database initialized");

    startupLogger.Information(
        "Configuring middleware pipeline (HTTPS redirection, routing, authentication, authorization)..."
    );

    app.UseAPI();
    app.MapControllers();

    startupLogger.Information("Task Manager API startup completed successfully");

    await app.StartAsync();

    startupLogger.Information("Application URLs: {Urls}", string.Join(", ", app.Urls));

    await app.WaitForShutdownAsync();
}
catch (HostAbortedException)
{
    startupLogger.Information("Host aborted by EF tools at design time, everything is fine");
}
catch (Exception exception)
{
    startupLogger.Fatal(exception, "Application terminated unexpectedly on startup");
}
finally
{
    startupLogger.Information("Shutting down Task Manager API. Flushing logs...");
    Log.CloseAndFlush();
}
