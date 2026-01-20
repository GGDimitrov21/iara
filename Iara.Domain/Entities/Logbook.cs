using Iara.Domain.Common;

namespace Iara.Domain.Entities;

/// <summary>
/// Logbook entry recording fishing activities
/// </summary>
public class Logbook : BaseEntity
{
    public int LogEntryId { get; set; }
    public int VesselId { get; set; }
    public int CaptainId { get; set; }
    public DateTime StartTime { get; set; }
    public int? DurationHours { get; set; }
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
    public int SpeciesId { get; set; }
    public decimal? CatchKg { get; set; }
    
    // Navigation properties
    public Vessel Vessel { get; set; } = null!;
    public Personnel Captain { get; set; } = null!;
    public Species Species { get; set; } = null!;
}
