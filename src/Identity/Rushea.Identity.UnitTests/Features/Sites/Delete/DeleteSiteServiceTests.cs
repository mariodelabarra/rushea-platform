using Microsoft.AspNetCore.Http.HttpResults;
using Rushea.Identity.API.Domain;
using Rushea.Identity.API.Features.Sites.Delete;
using Rushea.Identity.UnitTests.Common;
using Shouldly;

namespace Rushea.Identity.UnitTests.Features.Sites.Delete;

public class DeleteSiteServiceTests
{
    [Fact]
    public async Task DeleteAsync_WhenSiteExists_SetsStatusInactiveAndReturnsNoContent()
    {
        using var dbContext = IdentityDbContextFactory.Create();
        var site = new Site
        {
            OrganisationId = Guid.NewGuid(),
            Name = "Warehouse One",
            TimeZone = "UTC",
            BusinessDayCutOff = new TimeOnly(0, 0)
        };
        dbContext.Sites.Add(site);
        await dbContext.SaveChangesAsync();

        var sut = new DeleteSiteService(dbContext);

        var result = await sut.DeleteAsync(site.Id, CancellationToken.None);

        result.ShouldBeOfType<NoContent>();

        var persisted = dbContext.Sites.Single();
        persisted.Status.ShouldBe(SiteStatus.Inactive);
        persisted.UpdatedAtUtc.ShouldNotBeNull();
    }

    [Fact]
    public async Task DeleteAsync_WhenSiteDoesNotExist_ReturnsNotFound()
    {
        using var dbContext = IdentityDbContextFactory.Create();
        var sut = new DeleteSiteService(dbContext);

        var result = await sut.DeleteAsync(Guid.NewGuid(), CancellationToken.None);

        result.ShouldBeOfType<NotFound>();
    }
}
