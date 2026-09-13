using Microsoft.AspNetCore.Mvc;

namespace Rushea.Identity.API.Features.Sites.GetById;

[ApiController]
[Route("api/sites")]
public class GetSiteByIdController(IGetSiteByIdService service) : Controller
{
    [HttpGet("{id:guid}")]
    public async Task<IResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await service.GetByIdAsync(id, cancellationToken);

        return result;
    }
}
