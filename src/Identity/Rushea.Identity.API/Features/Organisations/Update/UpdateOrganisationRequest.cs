using FluentValidation;
using Rushea.Identity.API.Domain;

namespace Rushea.Identity.API.Features.Organisations.Update;

public record UpdateOrganisationRequest(string Name, OrganisationStatus Status);

public class UpdateOrganisationRequestValidator : AbstractValidator<UpdateOrganisationRequest>
{
    public UpdateOrganisationRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Status)
            .IsInEnum();
    }
}
