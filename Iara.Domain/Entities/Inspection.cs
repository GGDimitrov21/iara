using Iara.Domain.Common;

namespace Iara.Domain.Entities;

/// <summary>
/// Inspection record with violation tracking
/// </summary>
public class Inspection : BaseEntity
{
    public int InspectionId { get; set; }
    public int? InspectorId { get; set; }
    public int ShipId { get; set; }
    public DateTime InspectionDate { get; set; } = DateTime.UtcNow;
    public string? InspectionLocation { get; set; }
    public string ProtocolNumber { get; set; } = string.Empty;
    public bool HasViolation { get; set; }
    public string? ViolationDescription { get; set; }
    public string? SanctionsImposed { get; set; }
    public string? ProofOfViolationUrl { get; set; }
    public bool IsProcessed { get; set; } = false;
    
    // Navigation properties
    public User? Inspector { get; set; }
    public FishingShip Ship { get; set; } = null!;
}
