using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Rushea.Identity.API.Data;
using Rushea.Identity.API.Shared;
using Rushea.Identity.API.Shared.Validations;

using static Rushea.Identity.API.Shared.Extensions.ServiceCollectionExtensions;

namespace Rushea.Identity.API;

public static class DependencyInjection
{
    public static void ConfigureDependencies(this IServiceCollection services, ConfigurationManager configuration)
    {
        RegisterConfiguration(services, configuration);
        RegisterValidation(services);
        RegisterExceptionHandling(services);

        // Services Registrations
        RegisterOrganisationServices(services);
    }

    public static void RegisterValidation(IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
        services.AddScoped<IValidationService, ValidationService>();
    }

    public static void RegisterConfiguration(IServiceCollection services, ConfigurationManager configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<IdentityDbContext>(options =>
            options.UseNpgsql(connectionString));
    }

    public static void RegisterExceptionHandling(IServiceCollection services)
    {
        services.AddProblemDetails();
        services.AddExceptionHandler<ValidationExceptionHandler>();
    }
}
