namespace Iara.Application.DTOs.FishingShips;

/// <summary>
/// DTO for fishing ship response
/// </summary>
public record FishingShipDto
{
    public int ShipId { get; init; }
    public string IaraIdNumber { get; init; } = string.Empty;
    public string MaritimeNumber { get; init; } = string.Empty;
    public string ShipName { get; init; } = string.Empty;
    public string OwnerName { get; init; } = string.Empty;
    public decimal Tonnage { get; init; }
    public decimal ShipLength { get; init; }
    public decimal EnginePower { get; init; }
    public string? FuelType { get; init; }
    public DateOnly RegistrationDate { get; init; }
    public bool IsActive { get; init; }
}

/// <summary>
/// DTO for creating a new fishing ship
/// </summary>
public record CreateFishingShipRequest
{
    public string IaraIdNumber { get; init; } = string.Empty;
    public string MaritimeNumber { get; init; } = string.Empty;
    public string ShipName { get; init; } = string.Empty;
    public string OwnerName { get; init; } = string.Empty;
    public decimal Tonnage { get; init; }
    public decimal ShipLength { get; init; }
    public decimal EnginePower { get; init; }
    public string? FuelType { get; init; }
    public DateOnly RegistrationDate { get; init; }
}

/// <summary>
/// DTO for updating fishing ship details
/// </summary>
public record UpdateFishingShipRequest
{
    public string ShipName { get; init; } = string.Empty;
    public string OwnerName { get; init; } = string.Empty;
    public decimal Tonnage { get; init; }
    public decimal ShipLength { get; init; }
    public decimal EnginePower { get; init; }
    public string? FuelType { get; init; }
    public bool IsActive { get; init; }
}
