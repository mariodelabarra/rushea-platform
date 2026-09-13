using Microsoft.EntityFrameworkCore;
using Rushea.Identity.API.Data;
using Rushea.Identity.API.Features.Organisations.Shared.Common;

namespace Rushea.Identity.API.Features.Organisations.GetById;

public interface IGetOrganisationByIdService
{
    Task<IResult> GetByIdAsync(Guid id, CancellationToken cancellationToken);
}

public class GetOrganisationByIdService(IdentityDbContext dbContext) : IGetOrganisationByIdService
{
    public async Task<IResult> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var organisation = await dbContext.Organisations
            .AsNoTracking()
            .Where(o => o.Id == id)
            .Select(o => new OrganisationResponse(o.Id, o.Name, o.Status, o.CreatedAtUtc, o.UpdatedAtUtc))
            .FirstOrDefaultAsync(cancellationToken);

        return organisation is null ? Results.NotFound() : Results.Ok(organisation);
    }
}
