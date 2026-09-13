using Microsoft.EntityFrameworkCore;
using Rushea.Identity.API.Data;
using Rushea.Identity.API.Domain;

namespace Rushea.Identity.API.Features.Sites.Delete;

public interface IDeleteSiteService
{
    Task<IResult> DeleteAsync(Guid id, CancellationToken cancellationToken);
}

public class DeleteSiteService(IdentityDbContext dbContext) : IDeleteSiteService
{
    public async Task<IResult> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var site = await dbContext.Sites
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);

        if (site is null)
        {
            return Results.NotFound();
        }

        site.Status = SiteStatus.Inactive;
        site.UpdatedAtUtc = DateTimeOffset.UtcNow;

        await dbContext.SaveChangesAsync(cancellationToken);

        return Results.NoContent();
    }
}
