using FluentValidation;

namespace Rushea.Identity.API.Features.Sites.Update;

public record UpdateSiteRequest(string Name, string TimeZone, TimeOnly BusinessDayCutOff);

public class UpdateSiteRequestValidator : AbstractValidator<UpdateSiteRequest>
{
    public UpdateSiteRequestValidator()
    {
        RuleFor(s => s.Name)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(200);

        RuleFor(s => s.TimeZone)
            .NotEmpty()
            .Must(BeAValidTimeZone)
            .WithMessage("TimeZone must be a valid IANA time zone identifier.");
    }

    private static bool BeAValidTimeZone(string timeZone) =>
        TimeZoneInfo.TryFindSystemTimeZoneById(timeZone, out _);
}
