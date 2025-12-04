using Iara.Domain.Common;

namespace Iara.Domain.Entities;

/// <summary>
/// Fishing permit entity with quota and validity information
/// </summary>
public class FishingPermit : BaseEntity
{
    public int PermitId { get; set; }
    public int ShipId { get; set; }
    public int PermitYear { get; set; }
    public DateOnly IssueDate { get; set; }
    public DateOnly ValidFrom { get; set; }
    public DateOnly ValidUntil { get; set; }
    public string? CatchQuotaType { get; set; }
    public decimal? MinAnnualCatch { get; set; }
    public decimal? MaxAnnualCatch { get; set; }
    public decimal? TotalHoursAnnualLimit { get; set; }
    public string Status { get; set; } = string.Empty;
    public int? RegistrationDocumentId { get; set; }
    
    // Navigation properties
    public FishingShip Ship { get; set; } = null!;
    public Registration? RegistrationDocument { get; set; }
}

/// <summary>
/// Permit status values
/// </summary>
public static class PermitStatus
{
    public const string Active = "Active";
    public const string Expired = "Expired";
    public const string Suspended = "Suspended";
    public const string Revoked = "Revoked";
}
