using Rushea.Identity.API.Data;
using Rushea.Identity.API.Domain;
using Rushea.Identity.API.Features.Organisations.Shared.Common;
using Rushea.Identity.API.Shared.Validations;

namespace Rushea.Identity.API.Features.Organisations.Create;

public interface ICreateOrganisationService
{
    Task<IResult> CreateAsync(CreateOrganisationRequest request, CancellationToken cancellationToken);
}

public class CreateOrganisationService(IdentityDbContext dbContext, IValidationService validationService) : ICreateOrganisationService
{
    public async Task<IResult> CreateAsync(CreateOrganisationRequest request, CancellationToken cancellationToken)
    {
        await validationService.ValidateAndThrowAsync(request, cancellationToken);

        var organisation = new Organisation { Name = request.Name };

        dbContext.Organisations.Add(organisation);
        await dbContext.SaveChangesAsync(cancellationToken);

        var response = new OrganisationResponse(organisation.Id, organisation.Name, organisation.Status, organisation.CreatedAtUtc, organisation.UpdatedAtUtc);

        return Results.Created($"/api/organisations/{organisation.Id}", response);
    }
}
