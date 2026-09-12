using Microsoft.AspNetCore.Mvc;

namespace Rushea.Identity.API.Features.Organisations.GetAll;

[ApiController]
[Route("api/organisations")]
public class GetAllOrganisationsController(IGetAllOrganisationsService getAllOrganisationsService) : ControllerBase
{
    [HttpGet]
    public Task<IResult> GetAll(CancellationToken cancellationToken) =>
        getAllOrganisationsService.GetAllAsync(cancellationToken);
}
