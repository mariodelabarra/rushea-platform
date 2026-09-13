using Microsoft.EntityFrameworkCore;
using Rushea.Identity.API.Domain;

namespace Rushea.Identity.API.Data;

public class IdentityDbContext(DbContextOptions<IdentityDbContext> options) : DbContext(options)
{
    public DbSet<Organisation> Organisations => Set<Organisation>();
    public DbSet<Site> Sites => Set<Site>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("app");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(IdentityDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
