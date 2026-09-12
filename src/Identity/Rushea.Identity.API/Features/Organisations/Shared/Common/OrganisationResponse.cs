using Rushea.Identity.API.Domain;

namespace Rushea.Identity.API.Features.Organisations.Shared.Common;

public record OrganisationResponse(Guid Id, string Name, OrganisationStatus Status, DateTimeOffset CreatedAtUtc, DateTimeOffset? UpdatedAtUtc);
