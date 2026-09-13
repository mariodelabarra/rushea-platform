using Microsoft.AspNetCore.Http.HttpResults;
using Rushea.Identity.API.Domain;
using Rushea.Identity.API.Features.Organisations.Delete;
using Rushea.Identity.UnitTests.Common;
using Shouldly;

namespace Rushea.Identity.UnitTests.Features.Organisations.Delete;

public class DeleteOrganisationServiceTests
{
    [Fact]
    public async Task DeleteAsync_WhenOrganisationExists_SetsStatusSuspendedAndReturnsNoContent()
    {
        using var dbContext = IdentityDbContextFactory.Create();
        var organisation = new Organisation { Name = "Acme Corp" };
        dbContext.Organisations.Add(organisation);
        await dbContext.SaveChangesAsync();

        var sut = new DeleteOrganisationService(dbContext);

        var result = await sut.DeleteAsync(organisation.Id, CancellationToken.None);

        result.ShouldBeOfType<NoContent>();

        var persisted = dbContext.Organisations.Single();
        persisted.Status.ShouldBe(OrganisationStatus.Suspended);
        persisted.UpdatedAtUtc.ShouldNotBeNull();
    }

    [Fact]
    public async Task DeleteAsync_WhenOrganisationDoesNotExist_ReturnsNotFound()
    {
        using var dbContext = IdentityDbContextFactory.Create();
        var sut = new DeleteOrganisationService(dbContext);

        var result = await sut.DeleteAsync(Guid.NewGuid(), CancellationToken.None);

        result.ShouldBeOfType<NotFound>();
    }
}
