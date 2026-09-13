using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using Rushea.Identity.API.Domain;
using Rushea.Identity.API.Features.Sites.Create;
using Rushea.Identity.API.Features.Sites.Shared.Common;
using Rushea.Identity.API.Shared.Validations;
using Rushea.Identity.UnitTests.Common;
using Shouldly;

namespace Rushea.Identity.UnitTests.Features.Sites.Create;

public class CreateSitesServiceTests
{
    private readonly Mock<IValidationService> _validationService = new();

    [Fact]
    public async Task CreateAsync_WithValidRequest_PersistsSiteAndReturnsCreated()
    {
        using var dbContext = IdentityDbContextFactory.Create();
        _validationService
            .Setup(v => v.ValidateAndThrowAsync(It.IsAny<CreateSiteRequest>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var sut = new CreateSitesService(dbContext, _validationService.Object);
        var request = new CreateSiteRequest(Guid.NewGuid(), "Warehouse One", "Europe/Madrid", new TimeOnly(6, 0));

        var result = await sut.CreateAsync(request, CancellationToken.None);

        var created = result.ShouldBeOfType<Created<SiteResponse>>();
        created.Value.ShouldNotBeNull();
        created.Value.Name.ShouldBe(request.Name);
        created.Value.TimeZone.ShouldBe(request.TimeZone);
        created.Value.BusinessDayCutOff.ShouldBe(request.BusinessDayCutOff);
        created.Value.Status.ShouldBe(SiteStatus.Active);
        created.Location.ShouldBe($"/api/sites/{created.Value.Id}");

        var persisted = dbContext.Sites.Single();
        persisted.OrganisationId.ShouldBe(request.OrganisationId);
        persisted.Name.ShouldBe(request.Name);
    }

    [Fact]
    public async Task CreateAsync_WhenValidationFails_ThrowsAndDoesNotPersistSite()
    {
        using var dbContext = IdentityDbContextFactory.Create();
        _validationService
            .Setup(v => v.ValidateAndThrowAsync(It.IsAny<CreateSiteRequest>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new ValidationException([new ValidationFailure("Name", "'Name' must not be empty.")]));

        var sut = new CreateSitesService(dbContext, _validationService.Object);
        var request = new CreateSiteRequest(Guid.NewGuid(), string.Empty, "Europe/Madrid", new TimeOnly(6, 0));

        await Should.ThrowAsync<ValidationException>(() => sut.CreateAsync(request, CancellationToken.None));

        dbContext.Sites.ShouldBeEmpty();
    }
}
