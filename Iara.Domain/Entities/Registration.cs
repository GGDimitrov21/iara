using Iara.Domain.Common;

namespace Iara.Domain.Entities;

/// <summary>
/// Registration document entity for ships and permits
/// </summary>
public class Registration : BaseEntity
{
    public int RegistrationId { get; set; }
    public string DocumentType { get; set; } = string.Empty;
    public string IssuedBy { get; set; } = string.Empty;
    public DateOnly IssueDate { get; set; }
    public DateOnly ValidFrom { get; set; }
    public DateOnly ValidUntil { get; set; }
    public string? Description { get; set; }
    
    // Navigation properties
    public ICollection<FishingShip> FishingShips { get; set; } = new List<FishingShip>();
    public ICollection<FishingPermit> FishingPermits { get; set; } = new List<FishingPermit>();
}
