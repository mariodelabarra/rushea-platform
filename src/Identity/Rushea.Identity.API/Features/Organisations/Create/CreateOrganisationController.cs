using Microsoft.AspNetCore.Mvc;

namespace Rushea.Identity.API.Features.Organisations.Create;

[ApiController]
[Route("api/organisations")]
public class CreateOrganisationController(ICreateOrganisationService createOrganisationService) : ControllerBase
{
    [HttpPost]
    public async Task<IResult> Create(CreateOrganisationRequest request, CancellationToken cancellationToken)
    {
        var result = await createOrganisationService.CreateAsync(request, cancellationToken);

        return result;
    }
}
