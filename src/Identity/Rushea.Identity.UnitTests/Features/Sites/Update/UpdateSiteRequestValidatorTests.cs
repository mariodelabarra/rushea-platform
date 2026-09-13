using Rushea.Identity.API.Features.Sites.Update;
using Shouldly;

namespace Rushea.Identity.UnitTests.Features.Sites.Update;

public class UpdateSiteRequestValidatorTests
{
    private readonly UpdateSiteRequestValidator _sut = new();

    [Fact]
    public async Task Validate_WithValidRequest_IsValid()
    {
        var request = new UpdateSiteRequest("Warehouse One", "Europe/Madrid", new TimeOnly(6, 0));

        var result = await _sut.ValidateAsync(request);

        result.IsValid.ShouldBeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData("ab")]
    public async Task Validate_WithNameTooShort_IsInvalid(string name)
    {
        var request = new UpdateSiteRequest(name, "Europe/Madrid", new TimeOnly(6, 0));

        var result = await _sut.ValidateAsync(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == nameof(UpdateSiteRequest.Name));
    }

    [Fact]
    public async Task Validate_WithNameLongerThanMaximumLength_IsInvalid()
    {
        var request = new UpdateSiteRequest(new string('a', 201), "Europe/Madrid", new TimeOnly(6, 0));

        var result = await _sut.ValidateAsync(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == nameof(UpdateSiteRequest.Name));
    }

    [Theory]
    [InlineData("")]
    [InlineData("Not/AZone")]
    [InlineData("GMT+2")]
    public async Task Validate_WithInvalidTimeZone_IsInvalid(string timeZone)
    {
        var request = new UpdateSiteRequest("Warehouse One", timeZone, new TimeOnly(6, 0));

        var result = await _sut.ValidateAsync(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == nameof(UpdateSiteRequest.TimeZone));
    }
}
