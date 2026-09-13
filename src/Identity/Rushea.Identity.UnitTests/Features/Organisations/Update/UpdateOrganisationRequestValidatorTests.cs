using Rushea.Identity.API.Domain;
using Rushea.Identity.API.Features.Organisations.Update;
using Shouldly;

namespace Rushea.Identity.UnitTests.Features.Organisations.Update;

public class UpdateOrganisationRequestValidatorTests
{
    private readonly UpdateOrganisationRequestValidator _sut = new();

    [Fact]
    public async Task Validate_WithValidRequest_IsValid()
    {
        var request = new UpdateOrganisationRequest("Acme Corp", OrganisationStatus.Active);

        var result = await _sut.ValidateAsync(request);

        result.IsValid.ShouldBeTrue();
    }

    [Fact]
    public async Task Validate_WithEmptyName_IsInvalid()
    {
        var request = new UpdateOrganisationRequest(string.Empty, OrganisationStatus.Active);

        var result = await _sut.ValidateAsync(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == nameof(UpdateOrganisationRequest.Name));
    }

    [Fact]
    public async Task Validate_WithNameLongerThanMaximumLength_IsInvalid()
    {
        var request = new UpdateOrganisationRequest(new string('a', 201), OrganisationStatus.Active);

        var result = await _sut.ValidateAsync(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == nameof(UpdateOrganisationRequest.Name));
    }

    [Fact]
    public async Task Validate_WithUndefinedStatus_IsInvalid()
    {
        var request = new UpdateOrganisationRequest("Acme Corp", (OrganisationStatus)99);

        var result = await _sut.ValidateAsync(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == nameof(UpdateOrganisationRequest.Status));
    }
}
