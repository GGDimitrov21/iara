using Iara.Domain.Common;

namespace Iara.Domain.Entities;

/// <summary>
/// Inspection record with violation tracking
/// </summary>
public class Inspection : BaseEntity
{
    public int InspectionId { get; set; }
    public int VesselId { get; set; }
    public int InspectorId { get; set; }
    public DateTime InspectionDate { get; set; }
    public bool IsLegal { get; set; }
    public string? Notes { get; set; }
    
    // Navigation properties
    public Vessel Vessel { get; set; } = null!;
    public Personnel Inspector { get; set; } = null!;
    public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
