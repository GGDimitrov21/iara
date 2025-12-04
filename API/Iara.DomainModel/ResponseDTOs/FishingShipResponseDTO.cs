using System;

namespace Iara.DomainModel.ResponseDTOs
{
    public class FishingShipResponseDTO
    {
        public int ShipId { get; set; }
        public string IaraIdNumber { get; set; }
        public string MaritimeNumber { get; set; }
        public string ShipName { get; set; }
        public string OwnerName { get; set; }
        public decimal Tonnage { get; set; }
        public decimal ShipLength { get; set; }
        public decimal EnginePower { get; set; }
        public string FuelType { get; set; }
        public int? RegistrationDocumentId { get; set; }
        public DateTime RegistrationDate { get; set; }
        public bool IsActive { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}