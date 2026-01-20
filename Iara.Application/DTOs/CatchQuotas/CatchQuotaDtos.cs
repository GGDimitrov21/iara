namespace Iara.Application.DTOs.CatchQuotas;

/// <summary>
/// DTO for catch quota response
/// </summary>
public record CatchQuotaDto
{
    public int QuotaId { get; init; }
    public int PermitId { get; init; }
    public int SpeciesId { get; init; }
    public string SpeciesName { get; init; } = string.Empty;
    public short Year { get; init; }
    public decimal? MinCatchKg { get; init; }
    public decimal? AvgCatchKg { get; init; }
    public decimal MaxCatchKg { get; init; }
    public int? FuelHoursLimit { get; init; }
}

/// <summary>
/// DTO for creating a new catch quota
/// </summary>
public record CreateCatchQuotaRequest
{
    public int PermitId { get; init; }
    public int SpeciesId { get; init; }
    public short Year { get; init; }
    public decimal? MinCatchKg { get; init; }
    public decimal? AvgCatchKg { get; init; }
    public decimal MaxCatchKg { get; init; }
    public int? FuelHoursLimit { get; init; }
}

/// <summary>
/// DTO for updating catch quota
/// </summary>
public record UpdateCatchQuotaRequest
{
    public decimal? MinCatchKg { get; init; }
    public decimal? AvgCatchKg { get; init; }
    public decimal MaxCatchKg { get; init; }
    public int? FuelHoursLimit { get; init; }
}
