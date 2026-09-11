namespace Rushea.Identity.API.Domain.Common;

public interface IBaseEntity
{
    Guid Id { get; set; }
    DateTimeOffset CreatedAtUtc { get; set; }
    DateTimeOffset? UpdatedAtUtc { get; set; }
}

public abstract class BaseEntity : IBaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTimeOffset CreatedAtUtc { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? UpdatedAtUtc { get; set; }
}
