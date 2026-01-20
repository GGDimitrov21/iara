using Iara.Domain.Common;

namespace Iara.Domain.Entities;

/// <summary>
/// Catch quota entity representing fishing quotas per permit/species/year
/// </summary>
public class CatchQuota : BaseEntity
{
    public int QuotaId { get; set; }
    public int PermitId { get; set; }
    public int SpeciesId { get; set; }
    public short Year { get; set; }
    public decimal? MinCatchKg { get; set; }
    public decimal? AvgCatchKg { get; set; }
    public decimal MaxCatchKg { get; set; }
    public int? FuelHoursLimit { get; set; }
    
    // Navigation properties
    public Permit Permit { get; set; } = null!;
    public Species Species { get; set; } = null!;
}
