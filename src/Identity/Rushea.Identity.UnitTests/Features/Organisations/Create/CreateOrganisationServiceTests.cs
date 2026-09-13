using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using Rushea.Identity.API.Domain;
using Rushea.Identity.API.Features.Organisations.Create;
using Rushea.Identity.API.Features.Organisations.Shared.Common;
using Rushea.Identity.API.Shared.Validations;
using Rushea.Identity.UnitTests.Common;
using Shouldly;

namespace Rushea.Identity.UnitTests.Features.Organisations.Create;

public class CreateOrganisationServiceTests
{
    private readonly Mock<IValidationService> _validationService = new();

    [Fact]
    public async Task CreateAsync_WithValidRequest_PersistsOrganisationAndReturnsCreated()
    {
        using var dbContext = IdentityDbContextFactory.Create();
        _validationService
            .Setup(v => v.ValidateAndThrowAsync(It.IsAny<CreateOrganisationRequest>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var sut = new CreateOrganisationService(dbContext, _validationService.Object);
        var request = new CreateOrganisationRequest("Acme Corp");

        var result = await sut.CreateAsync(request, CancellationToken.None);

        var created = result.ShouldBeOfType<Created<OrganisationResponse>>();
        created.Value.ShouldNotBeNull();
        created.Value.Name.ShouldBe(request.Name);
        created.Value.Status.ShouldBe(OrganisationStatus.Active);
        created.Location.ShouldBe($"/api/organisations/{created.Value.Id}");

        var persisted = dbContext.Organisations.Single();
        persisted.Name.ShouldBe(request.Name);
    }

    [Fact]
    public async Task CreateAsync_WhenValidationFails_ThrowsAndDoesNotPersistOrganisation()
    {
        using var dbContext = IdentityDbContextFactory.Create();
        _validationService
            .Setup(v => v.ValidateAndThrowAsync(It.IsAny<CreateOrganisationRequest>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new ValidationException([new ValidationFailure("Name", "'Name' must not be empty.")]));

        var sut = new CreateOrganisationService(dbContext, _validationService.Object);
        var request = new CreateOrganisationRequest(string.Empty);

        await Should.ThrowAsync<ValidationException>(() => sut.CreateAsync(request, CancellationToken.None));

        dbContext.Organisations.ShouldBeEmpty();
    }
}
