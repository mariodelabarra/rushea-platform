using Microsoft.EntityFrameworkCore;
using Rushea.Identity.API.Data;

namespace Rushea.Identity.API;

public static class DependencyInjection
{
    public static void ConfigureDependencies(this IServiceCollection services, ConfigurationManager configuration)
    {
        RegisterConfiguration(services, configuration);
    }

    public static void RegisterConfiguration(IServiceCollection services, ConfigurationManager configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<IdentityDbContext>(options =>
            options.UseNpgsql(connectionString));
    }
}
