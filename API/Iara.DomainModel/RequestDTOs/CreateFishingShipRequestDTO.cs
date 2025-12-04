using System;

namespace Iara.DomainModel.RequestDTOs
{
    public class CreateFishingShipRequestDTO
    {
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
    }
}