namespace Iara.Domain.Common;

/// <summary>
/// Base entity class for all domain entities
/// </summary>
public abstract class BaseEntity
{
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}
