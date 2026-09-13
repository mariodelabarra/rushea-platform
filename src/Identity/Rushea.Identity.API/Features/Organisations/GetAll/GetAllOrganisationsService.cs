using Microsoft.EntityFrameworkCore;
using Rushea.Identity.API.Data;
using Rushea.Identity.API.Features.Organisations.Shared.Common;

namespace Rushea.Identity.API.Features.Organisations.GetAll;

public interface IGetAllOrganisationsService
{
    Task<IResult> GetAllAsync(CancellationToken cancellationToken);
}

public class GetAllOrganisationsService(IdentityDbContext dbContext) : IGetAllOrganisationsService
{
    public async Task<IResult> GetAllAsync(CancellationToken cancellationToken)
    {
        var organisations = await dbContext.Organisations
            .AsNoTracking()
            .Select(o => new OrganisationResponse(o.Id, o.Name, o.Status, o.CreatedAtUtc, o.UpdatedAtUtc))
            .ToListAsync(cancellationToken);

        return Results.Ok(organisations);
    }
}
