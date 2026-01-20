namespace Iara.Application.DTOs.Vessels;

/// <summary>
/// DTO for vessel response
/// </summary>
public record VesselDto
{
    public int VesselId { get; init; }
    public string RegNumber { get; init; } = string.Empty;
    public string VesselName { get; init; } = string.Empty;
    public string? OwnerDetails { get; init; }
    public int? CaptainId { get; init; }
    public string? CaptainName { get; init; }
    public decimal? LengthM { get; init; }
    public decimal? WidthM { get; init; }
    public decimal? Tonnage { get; init; }
    public string? FuelType { get; init; }
    public decimal? EnginePowerKw { get; init; }
    public decimal? DisplacementTons { get; init; }
}

/// <summary>
/// DTO for creating a new vessel
/// </summary>
public record CreateVesselRequest
{
    public string RegNumber { get; init; } = string.Empty;
    public string VesselName { get; init; } = string.Empty;
    public string? OwnerDetails { get; init; }
    public int? CaptainId { get; init; }
    public decimal? LengthM { get; init; }
    public decimal? WidthM { get; init; }
    public decimal? Tonnage { get; init; }
    public string? FuelType { get; init; }
    public decimal? EnginePowerKw { get; init; }
    public decimal? DisplacementTons { get; init; }
}

/// <summary>
/// DTO for updating vessel details
/// </summary>
public record UpdateVesselRequest
{
    public string VesselName { get; init; } = string.Empty;
    public string? OwnerDetails { get; init; }
    public int? CaptainId { get; init; }
    public decimal? LengthM { get; init; }
    public decimal? WidthM { get; init; }
    public decimal? Tonnage { get; init; }
    public string? FuelType { get; init; }
    public decimal? EnginePowerKw { get; init; }
    public decimal? DisplacementTons { get; init; }
}
