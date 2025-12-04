using Iara.Domain.Common;

namespace Iara.Domain.Entities;

/// <summary>
/// Annual ship classification based on engine hours
/// </summary>
public class ShipClassificationLog : BaseEntity
{
    public int LogId { get; set; }
    public int ShipId { get; set; }
    public int ClassificationYear { get; set; }
    public decimal TotalEngineHours { get; set; }
    public string? ClassificationLevel { get; set; }
    public DateOnly ClassificationDate { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);
    
    // Navigation properties
    public FishingShip Ship { get; set; } = null!;
}
