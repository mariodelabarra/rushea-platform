using Microsoft.AspNetCore.Http.HttpResults;
using Rushea.Identity.API.Domain;
using Rushea.Identity.API.Features.Sites.GetById;
using Rushea.Identity.API.Features.Sites.Shared.Common;
using Rushea.Identity.UnitTests.Common;
using Shouldly;

namespace Rushea.Identity.UnitTests.Features.Sites.GetById;

public class GetSiteByIdServiceTests
{
    [Fact]
    public async Task GetByIdAsync_WhenSiteExists_ReturnsOkWithSiteResponse()
    {
        using var dbContext = IdentityDbContextFactory.Create();
        var site = new Site
        {
            OrganisationId = Guid.NewGuid(),
            Name = "Warehouse One",
            TimeZone = "Europe/Madrid",
            BusinessDayCutOff = new TimeOnly(6, 0)
        };
        dbContext.Sites.Add(site);
        await dbContext.SaveChangesAsync();

        var sut = new GetSiteByIdService(dbContext);

        var result = await sut.GetByIdAsync(site.Id, CancellationToken.None);

        var ok = result.ShouldBeOfType<Ok<SiteResponse>>();
        ok.Value.ShouldNotBeNull();
        ok.Value.Id.ShouldBe(site.Id);
        ok.Value.Name.ShouldBe(site.Name);
        ok.Value.TimeZone.ShouldBe(site.TimeZone);
    }

    [Fact]
    public async Task GetByIdAsync_WhenSiteDoesNotExist_ReturnsNotFound()
    {
        using var dbContext = IdentityDbContextFactory.Create();
        var sut = new GetSiteByIdService(dbContext);

        var result = await sut.GetByIdAsync(Guid.NewGuid(), CancellationToken.None);

        result.ShouldBeOfType<NotFound>();
    }
}
