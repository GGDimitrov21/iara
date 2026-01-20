using Iara.Domain.Common;

namespace Iara.Domain.Entities;

/// <summary>
/// Species entity representing fish species
/// </summary>
public class Species : BaseEntity
{
    public int SpeciesId { get; set; }
    public string SpeciesName { get; set; } = string.Empty;
    
    // Navigation properties
    public ICollection<CatchQuota> CatchQuotas { get; set; } = new List<CatchQuota>();
    public ICollection<Logbook> LogbookEntries { get; set; } = new List<Logbook>();
}
