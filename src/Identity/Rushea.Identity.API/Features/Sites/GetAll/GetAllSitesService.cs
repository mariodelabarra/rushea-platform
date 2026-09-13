using Microsoft.EntityFrameworkCore;
using Rushea.Identity.API.Data;
using Rushea.Identity.API.Features.Sites.Shared.Common;

namespace Rushea.Identity.API.Features.Sites.GetAll;

public interface IGetAllSitesService
{
    Task<IResult> GetAllAsync(CancellationToken cancellationToken);
}

public class GetAllSitesService(IdentityDbContext dbContext) : IGetAllSitesService
{
    public async Task<IResult> GetAllAsync(CancellationToken cancellationToken)
    {
        var sites = await dbContext.Sites
            .AsNoTracking()
            .Select(s => new SiteResponse(s.Id, s.Name, s.TimeZone, s.BusinessDayCutOff, s.Status, s.CreatedAtUtc, s.UpdatedAtUtc))
            .ToListAsync(cancellationToken);

        return Results.Ok(sites);
    }
}
