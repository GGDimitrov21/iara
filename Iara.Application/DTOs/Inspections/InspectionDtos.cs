namespace Iara.Application.DTOs.Inspections;

/// <summary>
/// DTO for inspection response
/// </summary>
public record InspectionDto
{
    public int InspectionId { get; init; }
    public int? InspectorId { get; init; }
    public string? InspectorName { get; init; }
    public int ShipId { get; init; }
    public string ShipName { get; init; } = string.Empty;
    public DateTime InspectionDate { get; init; }
    public string? InspectionLocation { get; init; }
    public string ProtocolNumber { get; init; } = string.Empty;
    public bool HasViolation { get; init; }
    public string? ViolationDescription { get; init; }
    public string? SanctionsImposed { get; init; }
    public string? ProofOfViolationUrl { get; init; }
    public bool IsProcessed { get; init; }
}

/// <summary>
/// DTO for creating inspection record
/// </summary>
public record CreateInspectionRequest
{
    public int ShipId { get; init; }
    public string? InspectionLocation { get; init; }
    public string ProtocolNumber { get; init; } = string.Empty;
    public bool HasViolation { get; init; }
    public string? ViolationDescription { get; init; }
    public string? SanctionsImposed { get; init; }
    public string? ProofOfViolationUrl { get; init; }
}
