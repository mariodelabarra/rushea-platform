using Rushea.Identity.API.Domain.Common;

namespace Rushea.Identity.API.Domain;

public class Site : BaseEntity
{
    public Guid OrganisationId { get; init; }
    public required string Name { get; set; }
    public SiteStatus Status { get; set; } = SiteStatus.Active;
    public required string TimeZone { get; set; }
    public TimeOnly BusinessDayCutOff { get; set; } = new(0, 0);
}

public enum SiteStatus { Active, Inactive }
