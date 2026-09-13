using Microsoft.AspNetCore.Mvc;

namespace Rushea.Identity.API.Features.Sites.Update;

[ApiController]
[Route("api/sites")]
public class UpdateSiteController(IUpdateSiteService service) : ControllerBase
{
    [HttpPut("{id:guid}")]
    public async Task<IResult> Update(Guid id, UpdateSiteRequest request, CancellationToken cancellationToken)
    {
        var result = await service.UpdateAsync(id, request, cancellationToken);
        return result;
    }
}
