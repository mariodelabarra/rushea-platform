using Microsoft.EntityFrameworkCore;
using Rushea.Identity.API.Data;
using Rushea.Identity.API.Features.Organisations.Shared.Common;
using Rushea.Identity.API.Shared.Validations;

namespace Rushea.Identity.API.Features.Organisations.Update;

public interface IUpdateOrganisationService
{
    Task<IResult> UpdateAsync(Guid id, UpdateOrganisationRequest request, CancellationToken cancellationToken);
}

public class UpdateOrganisationService(IdentityDbContext dbContext, IValidationService validationService) : IUpdateOrganisationService
{
    public async Task<IResult> UpdateAsync(Guid id, UpdateOrganisationRequest request, CancellationToken cancellationToken)
    {
        await validationService.ValidateAndThrowAsync(request, cancellationToken);

        var organisation = await dbContext.Organisations.FirstOrDefaultAsync(org => org.Id == id, cancellationToken);

        if (organisation is null)
        {
            return Results.NotFound();
        }

        organisation.Name = request.Name;
        organisation.Status = request.Status;
        organisation.UpdatedAtUtc = DateTimeOffset.UtcNow;

        await dbContext.SaveChangesAsync(cancellationToken);

        var response = new OrganisationResponse(organisation.Id, organisation.Name, organisation.Status, organisation.CreatedAtUtc, organisation.UpdatedAtUtc);

        return Results.Ok(response);
    }
}
