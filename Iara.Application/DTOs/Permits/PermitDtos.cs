namespace Iara.Application.DTOs.Permits;

/// <summary>
/// DTO for permit response
/// </summary>
public record PermitDto
{
    public int PermitId { get; init; }
    public int VesselId { get; init; }
    public string VesselName { get; init; } = string.Empty;
    public DateTime IssueDate { get; init; }
    public DateTime ExpiryDate { get; init; }
    public bool IsActive { get; init; }
}

/// <summary>
/// DTO for creating a new permit
/// </summary>
public record CreatePermitRequest
{
    public int VesselId { get; init; }
    public DateTime IssueDate { get; init; }
    public DateTime ExpiryDate { get; init; }
}

/// <summary>
/// DTO for updating permit details
/// </summary>
public record UpdatePermitRequest
{
    public DateTime IssueDate { get; init; }
    public DateTime ExpiryDate { get; init; }
    public bool IsActive { get; init; }
}
