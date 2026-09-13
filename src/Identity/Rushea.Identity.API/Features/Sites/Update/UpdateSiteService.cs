using Microsoft.EntityFrameworkCore;
using Rushea.Identity.API.Data;
using Rushea.Identity.API.Features.Sites.Shared.Common;
using Rushea.Identity.API.Shared.Validations;

namespace Rushea.Identity.API.Features.Sites.Update;

public interface IUpdateSiteService
{
    Task<IResult> UpdateAsync(Guid id, UpdateSiteRequest request, CancellationToken cancellationToken);
}

public class UpdateSiteService(IdentityDbContext dbContext, IValidationService validationService) : IUpdateSiteService
{
    public async Task<IResult> UpdateAsync(Guid id, UpdateSiteRequest request, CancellationToken cancellationToken)
    {
        await validationService.ValidateAndThrowAsync(request, cancellationToken);

        var site = await dbContext.Sites.FirstOrDefaultAsync(s => s.Id == id, cancellationToken);

        if (site is null)
        {
            return Results.NotFound(id);
        }

        site.Name = request.Name;
        site.TimeZone = request.TimeZone;
        site.BusinessDayCutOff = request.BusinessDayCutOff;
        site.UpdatedAtUtc = DateTimeOffset.UtcNow;

        await dbContext.SaveChangesAsync(cancellationToken);

        var response = new SiteResponse(site.Id, site.Name, site.TimeZone, site.BusinessDayCutOff, site.Status, site.CreatedAtUtc, site.UpdatedAtUtc);

        return Results.Ok(response);
    }
}
