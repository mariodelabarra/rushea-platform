using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using Rushea.Identity.API.Domain;
using Rushea.Identity.API.Features.Organisations.Shared.Common;
using Rushea.Identity.API.Features.Organisations.Update;
using Rushea.Identity.API.Shared.Validations;
using Rushea.Identity.UnitTests.Common;
using Shouldly;

namespace Rushea.Identity.UnitTests.Features.Organisations.Update;

public class UpdateOrganisationServiceTests
{
    private readonly Mock<IValidationService> _validationService = new();

    [Fact]
    public async Task UpdateAsync_WithValidRequest_UpdatesOrganisationAndReturnsOk()
    {
        using var dbContext = IdentityDbContextFactory.Create();
        var organisation = new Organisation { Name = "Old Name" };
        dbContext.Organisations.Add(organisation);
        await dbContext.SaveChangesAsync();

        _validationService
            .Setup(v => v.ValidateAndThrowAsync(It.IsAny<UpdateOrganisationRequest>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var sut = new UpdateOrganisationService(dbContext, _validationService.Object);
        var request = new UpdateOrganisationRequest("New Name", OrganisationStatus.Suspended);

        var result = await sut.UpdateAsync(organisation.Id, request, CancellationToken.None);

        var ok = result.ShouldBeOfType<Ok<OrganisationResponse>>();
        ok.Value.ShouldNotBeNull();
        ok.Value.Name.ShouldBe(request.Name);
        ok.Value.Status.ShouldBe(request.Status);
        ok.Value.UpdatedAtUtc.ShouldNotBeNull();

        var persisted = dbContext.Organisations.Single();
        persisted.Name.ShouldBe(request.Name);
        persisted.Status.ShouldBe(request.Status);
        persisted.UpdatedAtUtc.ShouldNotBeNull();
    }

    [Fact]
    public async Task UpdateAsync_WhenOrganisationDoesNotExist_ReturnsNotFound()
    {
        using var dbContext = IdentityDbContextFactory.Create();
        _validationService
            .Setup(v => v.ValidateAndThrowAsync(It.IsAny<UpdateOrganisationRequest>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var sut = new UpdateOrganisationService(dbContext, _validationService.Object);
        var request = new UpdateOrganisationRequest("New Name", OrganisationStatus.Active);

        var result = await sut.UpdateAsync(Guid.NewGuid(), request, CancellationToken.None);

        result.ShouldBeOfType<NotFound>();
    }

    [Fact]
    public async Task UpdateAsync_WhenValidationFails_ThrowsAndDoesNotUpdateOrganisation()
    {
        using var dbContext = IdentityDbContextFactory.Create();
        var organisation = new Organisation { Name = "Old Name" };
        dbContext.Organisations.Add(organisation);
        await dbContext.SaveChangesAsync();

        _validationService
            .Setup(v => v.ValidateAndThrowAsync(It.IsAny<UpdateOrganisationRequest>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new ValidationException([new ValidationFailure("Name", "'Name' must not be empty.")]));

        var sut = new UpdateOrganisationService(dbContext, _validationService.Object);
        var request = new UpdateOrganisationRequest(string.Empty, OrganisationStatus.Active);

        await Should.ThrowAsync<ValidationException>(() => sut.UpdateAsync(organisation.Id, request, CancellationToken.None));

        var persisted = dbContext.Organisations.Single();
        persisted.Name.ShouldBe("Old Name");
        persisted.UpdatedAtUtc.ShouldBeNull();
    }
}
