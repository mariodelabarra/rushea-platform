using Microsoft.AspNetCore.Mvc;

namespace Rushea.Identity.API.Features.Sites.Delete;

[ApiController]
[Route("api/sites")]
public class DeleteSiteController(IDeleteSiteService service) : ControllerBase
{
    [HttpDelete("{id:guid}")]
    public async Task<IResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var result = await service.DeleteAsync(id, cancellationToken);

        return result;
    }
}
