using Rushea.Identity.API.Domain;
using Rushea.Identity.API.Features.Organisations.Create;
using Rushea.Identity.UnitTests.Common;
using Shouldly;

namespace Rushea.Identity.UnitTests.Features.Organisations.Create;

public class CreateOrganisationRequestValidatorTests
{
    [Fact]
    public async Task Validate_WithValidRequest_IsValid()
    {
        using var dbContext = IdentityDbContextFactory.Create();
        var sut = new CreateOrganisationRequestValidator(dbContext);
        var request = new CreateOrganisationRequest("Acme Corp");

        var result = await sut.ValidateAsync(request);

        result.IsValid.ShouldBeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData("ab")]
    public async Task Validate_WithNameTooShort_IsInvalid(string name)
    {
        using var dbContext = IdentityDbContextFactory.Create();
        var sut = new CreateOrganisationRequestValidator(dbContext);
        var request = new CreateOrganisationRequest(name);

        var result = await sut.ValidateAsync(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == nameof(CreateOrganisationRequest.Name));
    }

    [Fact]
    public async Task Validate_WithNameLongerThanMaximumLength_IsInvalid()
    {
        using var dbContext = IdentityDbContextFactory.Create();
        var sut = new CreateOrganisationRequestValidator(dbContext);
        var request = new CreateOrganisationRequest(new string('a', 201));

        var result = await sut.ValidateAsync(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == nameof(CreateOrganisationRequest.Name));
    }

    [Fact]
    public async Task Validate_WithDuplicateName_IsInvalid()
    {
        using var dbContext = IdentityDbContextFactory.Create();
        dbContext.Organisations.Add(new Organisation { Name = "Acme Corp" });
        await dbContext.SaveChangesAsync();

        var sut = new CreateOrganisationRequestValidator(dbContext);
        var request = new CreateOrganisationRequest("Acme Corp");

        var result = await sut.ValidateAsync(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == nameof(CreateOrganisationRequest.Name));
    }

    [Fact]
    public async Task Validate_WithDuplicateNameDifferentCase_IsInvalid()
    {
        using var dbContext = IdentityDbContextFactory.Create();
        dbContext.Organisations.Add(new Organisation { Name = "Acme Corp" });
        await dbContext.SaveChangesAsync();

        var sut = new CreateOrganisationRequestValidator(dbContext);
        var request = new CreateOrganisationRequest("ACME CORP");

        var result = await sut.ValidateAsync(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == nameof(CreateOrganisationRequest.Name));
    }
}
