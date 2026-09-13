using Microsoft.EntityFrameworkCore;
using Rushea.Identity.API.Data;

namespace Rushea.Identity.UnitTests.Common;

public static class IdentityDbContextFactory
{
    public static IdentityDbContext Create()
    {
        var options = new DbContextOptionsBuilder<IdentityDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new IdentityDbContext(options);
    }
}
