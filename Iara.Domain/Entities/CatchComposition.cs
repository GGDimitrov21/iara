using Iara.Domain.Common;

namespace Iara.Domain.Entities;

/// <summary>
/// Detailed catch composition for each log entry
/// </summary>
public class CatchComposition : BaseEntity
{
    public long CatchId { get; set; }
    public long LogEntryId { get; set; }
    public string FishSpecies { get; set; } = string.Empty;
    public decimal? WeightKg { get; set; }
    public int? Count { get; set; }
    public string? Status { get; set; }
    
    // Navigation properties
    public FishingLogEntry LogEntry { get; set; } = null!;
}

/// <summary>
/// Catch status values
/// </summary>
public static class CatchStatus
{
    public const string Kept = "Kept";
    public const string Released = "Released";
    public const string Discarded = "Discarded";
}
