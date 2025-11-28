using Backend.API.Extensions;
using Backend.Application.Extensions;
using Backend.Infastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder
    .Services.AddAPI(builder.Configuration)
    .AddInfastructure(builder.Configuration)
    .AddApplication();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapSwagger();
    app.UseSwaggerUI();
}

await app.Services.InitializeDatabaseAsync();

app.UseAPI();
app.UseHttpsRedirection();
app.MapControllers();

app.Run();
