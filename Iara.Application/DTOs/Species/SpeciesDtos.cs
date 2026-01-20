namespace Iara.Application.DTOs.Species;

/// <summary>
/// DTO for species response
/// </summary>
public record SpeciesDto
{
    public int SpeciesId { get; init; }
    public string SpeciesName { get; init; } = string.Empty;
}

/// <summary>
/// DTO for creating a new species
/// </summary>
public record CreateSpeciesRequest
{
    public string SpeciesName { get; init; } = string.Empty;
}
