using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using Rushea.Identity.API.Domain;
using Rushea.Identity.API.Features.Sites.Shared.Common;
using Rushea.Identity.API.Features.Sites.Update;
using Rushea.Identity.API.Shared.Validations;
using Rushea.Identity.UnitTests.Common;
using Shouldly;

namespace Rushea.Identity.UnitTests.Features.Sites.Update;

public class UpdateSiteServiceTests
{
    private readonly Mock<IValidationService> _validationService = new();

    [Fact]
    public async Task UpdateAsync_WithValidRequest_UpdatesSiteAndReturnsOk()
    {
        using var dbContext = IdentityDbContextFactory.Create();
        var site = new Site
        {
            OrganisationId = Guid.NewGuid(),
            Name = "Old Name",
            TimeZone = "UTC",
            BusinessDayCutOff = new TimeOnly(0, 0)
        };
        dbContext.Sites.Add(site);
        await dbContext.SaveChangesAsync();

        _validationService
            .Setup(v => v.ValidateAndThrowAsync(It.IsAny<UpdateSiteRequest>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var sut = new UpdateSiteService(dbContext, _validationService.Object);
        var request = new UpdateSiteRequest("New Name", "Europe/Madrid", new TimeOnly(6, 0));

        var result = await sut.UpdateAsync(site.Id, request, CancellationToken.None);

        var ok = result.ShouldBeOfType<Ok<SiteResponse>>();
        ok.Value.ShouldNotBeNull();
        ok.Value.Name.ShouldBe(request.Name);
        ok.Value.TimeZone.ShouldBe(request.TimeZone);
        ok.Value.BusinessDayCutOff.ShouldBe(request.BusinessDayCutOff);
        ok.Value.UpdatedAtUtc.ShouldNotBeNull();

        var persisted = dbContext.Sites.Single();
        persisted.Name.ShouldBe(request.Name);
        persisted.TimeZone.ShouldBe(request.TimeZone);
        persisted.UpdatedAtUtc.ShouldNotBeNull();
    }

    [Fact]
    public async Task UpdateAsync_WhenSiteDoesNotExist_ReturnsNotFound()
    {
        using var dbContext = IdentityDbContextFactory.Create();
        _validationService
            .Setup(v => v.ValidateAndThrowAsync(It.IsAny<UpdateSiteRequest>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var sut = new UpdateSiteService(dbContext, _validationService.Object);
        var missingId = Guid.NewGuid();
        var request = new UpdateSiteRequest("New Name", "Europe/Madrid", new TimeOnly(6, 0));

        var result = await sut.UpdateAsync(missingId, request, CancellationToken.None);

        var notFound = result.ShouldBeOfType<NotFound<Guid>>();
        notFound.Value.ShouldBe(missingId);
    }

    [Fact]
    public async Task UpdateAsync_WhenValidationFails_ThrowsAndDoesNotUpdateSite()
    {
        using var dbContext = IdentityDbContextFactory.Create();
        var site = new Site
        {
            OrganisationId = Guid.NewGuid(),
            Name = "Old Name",
            TimeZone = "UTC",
            BusinessDayCutOff = new TimeOnly(0, 0)
        };
        dbContext.Sites.Add(site);
        await dbContext.SaveChangesAsync();

        _validationService
            .Setup(v => v.ValidateAndThrowAsync(It.IsAny<UpdateSiteRequest>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new ValidationException([new ValidationFailure("Name", "'Name' must not be empty.")]));

        var sut = new UpdateSiteService(dbContext, _validationService.Object);
        var request = new UpdateSiteRequest(string.Empty, "Europe/Madrid", new TimeOnly(6, 0));

        await Should.ThrowAsync<ValidationException>(() => sut.UpdateAsync(site.Id, request, CancellationToken.None));

        var persisted = dbContext.Sites.Single();
        persisted.Name.ShouldBe("Old Name");
        persisted.UpdatedAtUtc.ShouldBeNull();
    }
}
