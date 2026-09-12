using Microsoft.AspNetCore.Mvc;

namespace Rushea.Identity.API.Features.Organisations.GetById;

[ApiController]
[Route("api/organisations")]
public class GetOrganisationByIdController(IGetOrganisationByIdService getOrganisationByIdService) : ControllerBase
{
    [HttpGet("{id:guid}")]
    public async Task<IResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await getOrganisationByIdService.GetByIdAsync(id, cancellationToken);

        return result;
    }
}
