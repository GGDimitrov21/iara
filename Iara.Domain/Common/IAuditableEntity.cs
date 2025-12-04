namespace Iara.Domain.Common;

/// <summary>
/// Interface for entities that require audit tracking
/// </summary>
public interface IAuditableEntity
{
    DateTime CreatedAt { get; set; }
    DateTime? UpdatedAt { get; set; }
    int? CreatedBy { get; set; }
    int? UpdatedBy { get; set; }
}
