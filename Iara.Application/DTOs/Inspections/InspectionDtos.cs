namespace Iara.Application.DTOs.Inspections;

/// <summary>
/// DTO for inspection response
/// </summary>
public record InspectionDto
{
    public int InspectionId { get; init; }
    public int VesselId { get; init; }
    public string VesselName { get; init; } = string.Empty;
    public int InspectorId { get; init; }
    public string InspectorName { get; init; } = string.Empty;
    public DateTime InspectionDate { get; init; }
    public bool IsLegal { get; init; }
    public string? Notes { get; init; }
}

/// <summary>
/// DTO for creating inspection record
/// </summary>
public record CreateInspectionRequest
{
    public int VesselId { get; init; }
    public int InspectorId { get; init; }
    public DateTime InspectionDate { get; init; }
    public bool IsLegal { get; init; }
    public string? Notes { get; init; }
}

/// <summary>
/// DTO for updating inspection record
/// </summary>
public record UpdateInspectionRequest
{
    public DateTime InspectionDate { get; init; }
    public bool IsLegal { get; init; }
    public string? Notes { get; init; }
}
