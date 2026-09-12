using FluentValidation;

namespace Rushea.Identity.API.Features.Organisations.Create;

public record CreateOrganisationRequest(string Name);

public class CreateOrganisationRequestValidator : AbstractValidator<CreateOrganisationRequest>
{
    public CreateOrganisationRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(200);
    }
}
