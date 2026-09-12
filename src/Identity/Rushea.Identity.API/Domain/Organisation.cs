using Rushea.Identity.API.Domain.Common;

namespace Rushea.Identity.API.Domain;

public class Organisation : BaseEntity
{
    public required string Name { get; set; }
    public OrganisationStatus Status { get; set; } = OrganisationStatus.Active;
}

public enum OrganisationStatus { Active, Suspended }
