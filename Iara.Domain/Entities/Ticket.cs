using Iara.Domain.Common;

namespace Iara.Domain.Entities;

/// <summary>
/// Ticket entity for validation records
/// </summary>
public class Ticket : BaseEntity
{
    public int TicketId { get; set; }
    public string TicketNumber { get; set; } = string.Empty;
    public DateTime? ExpiryDate { get; set; }
    public string PersonStatus { get; set; } = string.Empty;
    public bool IsValidated { get; set; } = false;
    public DateTime? ValidationDate { get; set; }
    public int? InspectionId { get; set; }
    
    // Navigation properties
    public Inspection? Inspection { get; set; }
}

/// <summary>
/// Person status values for tickets
/// </summary>
public static class PersonStatusTypes
{
    public const string Standard = "Standard";
    public const string Under14 = "Under 14";
    public const string Pensioner = "Pensioner";
}
