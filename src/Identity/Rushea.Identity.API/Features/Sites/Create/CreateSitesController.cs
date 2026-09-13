using Microsoft.AspNetCore.Mvc;

namespace Rushea.Identity.API.Features.Sites.Create;

[ApiController]
[Route("api/sites")]
public class CreateSitesController(ICreateSitesService service) : ControllerBase
{
    [HttpPost]
    public async Task<IResult> Create(CreateSiteRequest request, CancellationToken cancellationToken)
    {
        var result = await service.CreateAsync(request, cancellationToken);

        return result;
    }
}
