namespace Iara.Application.DTOs.FishingLogs;

/// <summary>
/// DTO for catch composition
/// </summary>
public record CatchCompositionDto
{
    public long CatchId { get; init; }
    public string FishSpecies { get; init; } = string.Empty;
    public decimal? WeightKg { get; init; }
    public int? Count { get; init; }
    public string? Status { get; init; }
}

/// <summary>
/// DTO for fishing log entry response
/// </summary>
public record FishingLogEntryDto
{
    public long LogEntryId { get; init; }
    public int ShipId { get; init; }
    public string ShipName { get; init; } = string.Empty;
    public DateOnly LogDate { get; init; }
    public TimeOnly? StartTime { get; init; }
    public TimeOnly? EndTime { get; init; }
    public string? FishingZone { get; init; }
    public string? CatchDetails { get; init; }
    public string? RouteDetails { get; init; }
    public bool IsSigned { get; init; }
    public DateTime SubmittedAt { get; init; }
    public List<CatchCompositionDto> CatchCompositions { get; init; } = new();
}

/// <summary>
/// DTO for creating fishing log entry
/// </summary>
public record CreateFishingLogEntryRequest
{
    public int ShipId { get; init; }
    public DateOnly LogDate { get; init; }
    public TimeOnly? StartTime { get; init; }
    public TimeOnly? EndTime { get; init; }
    public string? FishingZone { get; init; }
    public string? CatchDetails { get; init; }
    public string? RouteDetails { get; init; }
    public List<CreateCatchCompositionRequest> CatchCompositions { get; init; } = new();
}

/// <summary>
/// DTO for creating catch composition
/// </summary>
public record CreateCatchCompositionRequest
{
    public string FishSpecies { get; init; } = string.Empty;
    public decimal? WeightKg { get; init; }
    public int? Count { get; init; }
    public string? Status { get; init; }
}
