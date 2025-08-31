using DbContexts;
using Microsoft.EntityFrameworkCore;
using Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

var allowedOrigin = builder.Configuration["Cors:AllowedOrigin"] ?? throw new Exception("Cors:AllowedOrigin not set");
builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowFrontend", policy =>
            policy.WithOrigins(allowedOrigin)
            .AllowAnyMethod()
            .AllowAnyHeader());
    }
);

var connectionString = builder.Configuration["DATABASE_URL"] ?? throw new Exception("DATABASE_URL not set");
builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));

builder.Services.AddScoped<ITagService, TagService>();
builder.Services.AddScoped<ITaskStateService, TaskStateService>();
builder.Services.AddScoped<ITaskService, TaskService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();
}

using var scope = app.Services.CreateScope();
var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
db.Database.Migrate();

app.UseCors("AllowFrontend");
app.UseHttpsRedirection();
app.MapControllers();

app.Run();
