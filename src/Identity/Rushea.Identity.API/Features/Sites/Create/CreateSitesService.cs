using Rushea.Identity.API.Data;
using Rushea.Identity.API.Domain;
using Rushea.Identity.API.Features.Sites.Shared.Common;
using Rushea.Identity.API.Shared.Validations;

namespace Rushea.Identity.API.Features.Sites.Create;

public interface ICreateSitesService
{
    Task<IResult> CreateAsync(CreateSiteRequest request, CancellationToken cancellationToken);
}

public class CreateSitesService(IdentityDbContext dbContext, IValidationService validationService) : ICreateSitesService
{
    public async Task<IResult> CreateAsync(CreateSiteRequest request, CancellationToken cancellationToken)
    {
        await validationService.ValidateAndThrowAsync(request, cancellationToken);

        var site = new Site
        {
            OrganisationId = request.OrganisationId,
            Name = request.Name,
            TimeZone = request.TimeZone,
            BusinessDayCutOff = request.BusinessDayCutOff
        };

        dbContext.Sites.Add(site);
        await dbContext.SaveChangesAsync(cancellationToken);

        var response = new SiteResponse(site.Id,
            site.Name,
            site.TimeZone,
            site.BusinessDayCutOff,
            site.Status,
            site.CreatedAtUtc,
            site.UpdatedAtUtc);

        return Results.Created($"/api/sites/{site.Id}", response);
    }
}
