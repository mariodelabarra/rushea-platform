using Scalar.AspNetCore;

using Rushea.Identity.API;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
DependencyInjection.ConfigureDependencies(builder.Services, builder.Configuration);

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}
app.MapGet("/", () => "Hello world!");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
