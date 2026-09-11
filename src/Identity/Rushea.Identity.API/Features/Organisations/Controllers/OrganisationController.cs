using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rushea.Identity.API.Data;

namespace Rushea.Identity.API.Features.Organisations.Controllers;

[ApiController]
[Route("api/organisations")]
public class OrganisationController(IdentityDbContext dbContext) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var organisations = await dbContext.Organisations
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return Ok(organisations);
    }
}
