using Iara.Domain.Common;

namespace Iara.Domain.Entities;

/// <summary>
/// Vessel entity with registration and technical details
/// </summary>
public class Vessel : BaseEntity
{
    public int VesselId { get; set; }
    public string RegNumber { get; set; } = string.Empty;
    public string VesselName { get; set; } = string.Empty;
    public string? OwnerDetails { get; set; }
    public int? CaptainId { get; set; }
    public decimal? LengthM { get; set; }
    public decimal? WidthM { get; set; }
    public decimal? Tonnage { get; set; }
    public string? FuelType { get; set; }
    public decimal? EnginePowerKw { get; set; }
    public decimal? DisplacementTons { get; set; }
    
    // Navigation properties
    public Personnel? Captain { get; set; }
    public ICollection<Permit> Permits { get; set; } = new List<Permit>();
    public ICollection<Logbook> LogbookEntries { get; set; } = new List<Logbook>();
    public ICollection<Inspection> Inspections { get; set; } = new List<Inspection>();
}
