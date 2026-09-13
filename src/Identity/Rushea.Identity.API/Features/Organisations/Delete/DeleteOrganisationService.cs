using Microsoft.EntityFrameworkCore;
using Rushea.Identity.API.Data;
using Rushea.Identity.API.Domain;

namespace Rushea.Identity.API.Features.Organisations.Delete;

public interface IDeleteOrganisationService
{
    Task<IResult> DeleteAsync(Guid id, CancellationToken cancellationToken);
}

public class DeleteOrganisationService(IdentityDbContext dbContext) : IDeleteOrganisationService
{
    public async Task<IResult> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var organisation = await dbContext.Organisations
            .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);

        if (organisation is null)
        {
            return Results.NotFound();
        }
        organisation.Status = OrganisationStatus.Suspended;
        organisation.UpdatedAtUtc = DateTimeOffset.UtcNow;
        await dbContext.SaveChangesAsync(cancellationToken);

        return Results.NoContent();
    }
}
