using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Rushea.Identity.API.Data;

namespace Rushea.Identity.API.Features.Organisations.Create;

public record CreateOrganisationRequest(string Name);

public class CreateOrganisationRequestValidator : AbstractValidator<CreateOrganisationRequest>
{
    public CreateOrganisationRequestValidator(IdentityDbContext dbContext)
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(200)
            .MustAsync((name, cancellationToken) => BeAUniqueNameAsync(dbContext, name, cancellationToken))
            .WithMessage("An organisation with this name already exists.");
    }

    private static async Task<bool> BeAUniqueNameAsync(IdentityDbContext dbContext, string name, CancellationToken cancellationToken)
    {
        var normalizedName = name.ToLowerInvariant();

        return !await dbContext.Organisations
            .AnyAsync(o => o.Name.ToLower() == normalizedName, cancellationToken);
    }
}
