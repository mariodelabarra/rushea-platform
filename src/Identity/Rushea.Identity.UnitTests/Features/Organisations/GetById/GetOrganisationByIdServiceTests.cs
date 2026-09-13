using Microsoft.AspNetCore.Http.HttpResults;
using Rushea.Identity.API.Domain;
using Rushea.Identity.API.Features.Organisations.GetById;
using Rushea.Identity.API.Features.Organisations.Shared.Common;
using Rushea.Identity.UnitTests.Common;
using Shouldly;

namespace Rushea.Identity.UnitTests.Features.Organisations.GetById;

public class GetOrganisationByIdServiceTests
{
    [Fact]
    public async Task GetByIdAsync_WhenOrganisationExists_ReturnsOkWithOrganisationResponse()
    {
        using var dbContext = IdentityDbContextFactory.Create();
        var organisation = new Organisation { Name = "Acme Corp" };
        dbContext.Organisations.Add(organisation);
        await dbContext.SaveChangesAsync();

        var sut = new GetOrganisationByIdService(dbContext);

        var result = await sut.GetByIdAsync(organisation.Id, CancellationToken.None);

        var ok = result.ShouldBeOfType<Ok<OrganisationResponse>>();
        ok.Value.ShouldNotBeNull();
        ok.Value.Id.ShouldBe(organisation.Id);
        ok.Value.Name.ShouldBe(organisation.Name);
    }

    [Fact]
    public async Task GetByIdAsync_WhenOrganisationDoesNotExist_ReturnsNotFound()
    {
        using var dbContext = IdentityDbContextFactory.Create();
        var sut = new GetOrganisationByIdService(dbContext);

        var result = await sut.GetByIdAsync(Guid.NewGuid(), CancellationToken.None);

        result.ShouldBeOfType<NotFound>();
    }
}
