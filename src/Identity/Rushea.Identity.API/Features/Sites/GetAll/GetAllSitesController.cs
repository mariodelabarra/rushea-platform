using Microsoft.AspNetCore.Mvc;

namespace Rushea.Identity.API.Features.Sites.GetAll;

[ApiController]
[Route("api/sites")]
public class GetAllSitesController(IGetAllSitesService service) : ControllerBase
{
    [HttpGet]
    public Task<IResult> GetAll(CancellationToken cancellationToken) =>
        service.GetAllAsync(cancellationToken);
}
