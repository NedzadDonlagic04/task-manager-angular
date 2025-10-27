using Backend.API.Extensions;
using Backend.Application.Extensions;
using Backend.Infastructure.Extensions;

namespace Backend.API;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddSwaggerGen();
        builder.Services.AddAPI(builder.Configuration);
        builder.Services.AddInfastructure(builder.Configuration);
        builder.Services.AddApplication();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            app.MapSwagger();
        }

        await app.Services.InitializeDatabaseAsync();

        app.UseCors("AllowFrontend");
        app.UseHttpsRedirection();
        app.MapControllers();

        app.Run();
    }
}
