using System;

namespace Iara.DomainModel.RequestDTOs
{
    public class UpdateFishingShipRequestDTO
    {
        public string ShipName { get; set; }
        public string OwnerName { get; set; }
        public decimal? Tonnage { get; set; }
        public decimal? ShipLength { get; set; }
        public decimal? EnginePower { get; set; }
        public string FuelType { get; set; }
        public bool? IsActive { get; set; }
    }
}