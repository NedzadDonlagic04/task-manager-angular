using Backend.API.Extensions;
using Backend.Application.Extensions;
using Backend.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder
    .Services.AddAPI(builder.Configuration)
    .AddInfastructure(builder.Configuration)
    .AddApplication();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapSwagger().AllowAnonymous();
    app.UseSwaggerUI();
}

await app.Services.InitializeDatabaseAsync(app.Environment);

app.UseHttpsRedirection();
app.UseAPI();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
