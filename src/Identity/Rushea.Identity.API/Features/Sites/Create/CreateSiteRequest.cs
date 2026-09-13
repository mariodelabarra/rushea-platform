using FluentValidation;

namespace Rushea.Identity.API.Features.Sites.Create;

public record CreateSiteRequest(Guid OrganisationId, string Name, string TimeZone, TimeOnly BusinessDayCutOff);

public class CreateSiteRequestValidator : AbstractValidator<CreateSiteRequest>
{
    public CreateSiteRequestValidator()
    {
        RuleFor(x => x.OrganisationId)
            .NotEmpty();

        RuleFor(x => x.Name)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(200);

        RuleFor(x => x.TimeZone)
            .NotEmpty()
            .Must(BeAValidTimeZone)
            .WithMessage("TimeZone must be a valid IANA time zone identifier.");
    }

    private static bool BeAValidTimeZone(string timeZone) =>
        TimeZoneInfo.TryFindSystemTimeZoneById(timeZone, out _);
}
