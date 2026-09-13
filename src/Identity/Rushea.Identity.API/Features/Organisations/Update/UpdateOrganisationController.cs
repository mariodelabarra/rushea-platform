using Microsoft.AspNetCore.Mvc;

namespace Rushea.Identity.API.Features.Organisations.Update;

[ApiController]
[Route("api/organisations")]
public class UpdateOrganisationController(IUpdateOrganisationService updateOrganisationService) : ControllerBase
{
    [HttpPut("{id:guid}")]
    public async Task<IResult> Update(Guid id, UpdateOrganisationRequest request, CancellationToken cancellationToken)
    {
        var result = await updateOrganisationService.UpdateAsync(id, request, cancellationToken);
        return result;
    }
}
