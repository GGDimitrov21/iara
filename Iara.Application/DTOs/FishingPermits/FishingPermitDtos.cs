namespace Iara.Application.DTOs.FishingPermits;

/// <summary>
/// DTO for fishing permit response
/// </summary>
public record FishingPermitDto
{
    public int PermitId { get; init; }
    public int ShipId { get; init; }
    public string ShipName { get; init; } = string.Empty;
    public int PermitYear { get; init; }
    public DateOnly IssueDate { get; init; }
    public DateOnly ValidFrom { get; init; }
    public DateOnly ValidUntil { get; init; }
    public string? CatchQuotaType { get; init; }
    public decimal? MinAnnualCatch { get; init; }
    public decimal? MaxAnnualCatch { get; init; }
    public decimal? TotalHoursAnnualLimit { get; init; }
    public string Status { get; init; } = string.Empty;
}

/// <summary>
/// DTO for creating a new fishing permit
/// </summary>
public record CreateFishingPermitRequest
{
    public int ShipId { get; init; }
    public int PermitYear { get; init; }
    public DateOnly ValidFrom { get; init; }
    public DateOnly ValidUntil { get; init; }
    public string? CatchQuotaType { get; init; }
    public decimal? MinAnnualCatch { get; init; }
    public decimal? MaxAnnualCatch { get; init; }
    public decimal? TotalHoursAnnualLimit { get; init; }
}
