using Iara.Domain.Common;

namespace Iara.Domain.Entities;

/// <summary>
/// Permit entity with validity information
/// </summary>
public class Permit : BaseEntity
{
    public int PermitId { get; set; }
    public int VesselId { get; set; }
    public DateTime IssueDate { get; set; }
    public DateTime ExpiryDate { get; set; }
    public bool IsActive { get; set; } = true;
    
    // Navigation properties
    public Vessel Vessel { get; set; } = null!;
    public ICollection<CatchQuota> CatchQuotas { get; set; } = new List<CatchQuota>();
}
