using Microsoft.AspNetCore.Mvc;

namespace Rushea.Identity.API.Features.Organisations.Delete;

[ApiController]
[Route("api/organisations")]
public class DeleteOrganisationController(IDeleteOrganisationService deleteOrganisationService) : ControllerBase
{
    [HttpDelete("{id:guid}")]
    public async Task<IResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var result = await deleteOrganisationService.DeleteAsync(id, cancellationToken);

        return result;
    }
}
