namespace Iara.Application.DTOs.Logbook;

/// <summary>
/// DTO for logbook entry response
/// </summary>
public record LogbookEntryDto
{
    public int LogEntryId { get; init; }
    public int VesselId { get; init; }
    public string VesselName { get; init; } = string.Empty;
    public int CaptainId { get; init; }
    public string CaptainName { get; init; } = string.Empty;
    public DateTime StartTime { get; init; }
    public int? DurationHours { get; init; }
    public decimal? Latitude { get; init; }
    public decimal? Longitude { get; init; }
    public int SpeciesId { get; init; }
    public string SpeciesName { get; init; } = string.Empty;
    public decimal? CatchKg { get; init; }
}

/// <summary>
/// DTO for creating a new logbook entry
/// </summary>
public record CreateLogbookEntryRequest
{
    public int VesselId { get; init; }
    public int CaptainId { get; init; }
    public DateTime StartTime { get; init; }
    public int? DurationHours { get; init; }
    public decimal? Latitude { get; init; }
    public decimal? Longitude { get; init; }
    public int SpeciesId { get; init; }
    public decimal? CatchKg { get; init; }
}

/// <summary>
/// DTO for updating logbook entry details
/// </summary>
public record UpdateLogbookEntryRequest
{
    public DateTime StartTime { get; init; }
    public int? DurationHours { get; init; }
    public decimal? Latitude { get; init; }
    public decimal? Longitude { get; init; }
    public int SpeciesId { get; init; }
    public decimal? CatchKg { get; init; }
}
