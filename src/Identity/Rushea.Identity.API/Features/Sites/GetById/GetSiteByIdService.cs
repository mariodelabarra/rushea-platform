using Microsoft.EntityFrameworkCore;
using Rushea.Identity.API.Data;
using Rushea.Identity.API.Features.Sites.Shared.Common;

namespace Rushea.Identity.API.Features.Sites.GetById;

public interface IGetSiteByIdService
{
    Task<IResult> GetByIdAsync(Guid id, CancellationToken cancellationToken);
}
public class GetSiteByIdService(IdentityDbContext dbContext) : IGetSiteByIdService
{
    public async Task<IResult> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var site = await dbContext.Sites
            .AsNoTracking()
            .Where(s => s.Id == id)
            .Select(s => new SiteResponse(s.Id, s.Name, s.TimeZone, s.BusinessDayCutOff, s.Status, s.CreatedAtUtc, s.UpdatedAtUtc))
            .FirstOrDefaultAsync(cancellationToken);

        return site is null ? Results.NotFound() : Results.Ok(site);
    }
}
