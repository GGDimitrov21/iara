using Iara.Domain.Common;

namespace Iara.Domain.Entities;

/// <summary>
/// Daily fishing log entry with activity details
/// </summary>
public class FishingLogEntry : BaseEntity
{
    public long LogEntryId { get; set; }
    public int ShipId { get; set; }
    public DateOnly LogDate { get; set; }
    public TimeOnly? StartTime { get; set; }
    public TimeOnly? EndTime { get; set; }
    public string? FishingZone { get; set; }
    public string? CatchDetails { get; set; }
    public string? RouteDetails { get; set; }
    public bool IsSigned { get; set; } = false;
    public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;
    
    // Navigation properties
    public FishingShip Ship { get; set; } = null!;
    public ICollection<CatchComposition> CatchCompositions { get; set; } = new List<CatchComposition>();
}
