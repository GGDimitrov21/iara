using Iara.Domain.Common;

namespace Iara.Domain.Entities;

/// <summary>
/// Fishing ship entity with registration and technical details
/// </summary>
public class FishingShip : BaseEntity
{
    public int ShipId { get; set; }
    public string IaraIdNumber { get; set; } = string.Empty;
    public string MaritimeNumber { get; set; } = string.Empty;
    public string ShipName { get; set; } = string.Empty;
    public string OwnerName { get; set; } = string.Empty;
    public decimal Tonnage { get; set; }
    public decimal ShipLength { get; set; }
    public decimal EnginePower { get; set; }
    public string? FuelType { get; set; }
    public int? RegistrationDocumentId { get; set; }
    public DateOnly RegistrationDate { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
    
    // Navigation properties
    public Registration? RegistrationDocument { get; set; }
    public ICollection<FishingPermit> FishingPermits { get; set; } = new List<FishingPermit>();
    public ICollection<FishingLogEntry> FishingLogEntries { get; set; } = new List<FishingLogEntry>();
    public ICollection<Inspection> Inspections { get; set; } = new List<Inspection>();
    public ICollection<ShipClassificationLog> ClassificationLogs { get; set; } = new List<ShipClassificationLog>();
}
