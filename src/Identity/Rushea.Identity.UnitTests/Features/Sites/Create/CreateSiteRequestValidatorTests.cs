using Rushea.Identity.API.Features.Sites.Create;
using Shouldly;

namespace Rushea.Identity.UnitTests.Features.Sites.Create;

public class CreateSiteRequestValidatorTests
{
    private readonly CreateSiteRequestValidator _sut = new();

    [Fact]
    public async Task Validate_WithValidRequest_IsValid()
    {
        var request = new CreateSiteRequest(Guid.NewGuid(), "Warehouse One", "Europe/Madrid", new TimeOnly(6, 0));

        var result = await _sut.ValidateAsync(request);

        result.IsValid.ShouldBeTrue();
    }

    [Fact]
    public async Task Validate_WithEmptyOrganisationId_IsInvalid()
    {
        var request = new CreateSiteRequest(Guid.Empty, "Warehouse One", "Europe/Madrid", new TimeOnly(6, 0));

        var result = await _sut.ValidateAsync(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == nameof(CreateSiteRequest.OrganisationId));
    }

    [Theory]
    [InlineData("")]
    [InlineData("ab")]
    public async Task Validate_WithNameTooShort_IsInvalid(string name)
    {
        var request = new CreateSiteRequest(Guid.NewGuid(), name, "Europe/Madrid", new TimeOnly(6, 0));

        var result = await _sut.ValidateAsync(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == nameof(CreateSiteRequest.Name));
    }

    [Fact]
    public async Task Validate_WithNameLongerThanMaximumLength_IsInvalid()
    {
        var request = new CreateSiteRequest(Guid.NewGuid(), new string('a', 201), "Europe/Madrid", new TimeOnly(6, 0));

        var result = await _sut.ValidateAsync(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == nameof(CreateSiteRequest.Name));
    }

    [Fact]
    public async Task Validate_WithEmptyTimeZone_IsInvalid()
    {
        var request = new CreateSiteRequest(Guid.NewGuid(), "Warehouse One", string.Empty, new TimeOnly(6, 0));

        var result = await _sut.ValidateAsync(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == nameof(CreateSiteRequest.TimeZone));
    }

    [Theory]
    [InlineData("Not/AZone")]
    [InlineData("Madrid")]
    [InlineData("GMT+2")]
    public async Task Validate_WithInvalidTimeZone_IsInvalid(string timeZone)
    {
        var request = new CreateSiteRequest(Guid.NewGuid(), "Warehouse One", timeZone, new TimeOnly(6, 0));

        var result = await _sut.ValidateAsync(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == nameof(CreateSiteRequest.TimeZone));
    }

    [Theory]
    [InlineData("UTC")]
    [InlineData("Europe/Madrid")]
    [InlineData("America/New_York")]
    public async Task Validate_WithValidIanaTimeZone_IsValid(string timeZone)
    {
        var request = new CreateSiteRequest(Guid.NewGuid(), "Warehouse One", timeZone, new TimeOnly(6, 0));

        var result = await _sut.ValidateAsync(request);

        result.IsValid.ShouldBeTrue();
    }
}
