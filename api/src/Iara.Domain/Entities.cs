namespace Iara.Domain.Entities;

public class Vessel
{
    public int VesselId { get; set; }
    public string RegNumber { get; set; } = string.Empty;
    public string VesselName { get; set; } = string.Empty;
}

public class Species
{
    public int SpeciesId { get; set; }
    public string SpeciesName { get; set; } = string.Empty;
}
