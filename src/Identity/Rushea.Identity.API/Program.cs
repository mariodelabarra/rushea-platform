using System.Text.Json.Serialization;

using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

using Rushea.Identity.API;
using Rushea.Identity.API.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
DependencyInjection.ConfigureDependencies(builder.Services, builder.Configuration);

builder.Services.AddControllers();
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
    options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

app.UseExceptionHandler();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();

    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<IdentityDbContext>();
    dbContext.Database.Migrate();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
