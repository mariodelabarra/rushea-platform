using Rushea.Identity.API.Domain;

namespace Rushea.Identity.API.Features.Sites.Shared.Common;

public record SiteResponse(
    Guid Id,
    string Name,
    string TimeZone,
    TimeOnly BusinessDayCutOff,
    SiteStatus Status,
    DateTimeOffset CreatedAtUtc,
    DateTimeOffset? UpdatedAtUtc);
