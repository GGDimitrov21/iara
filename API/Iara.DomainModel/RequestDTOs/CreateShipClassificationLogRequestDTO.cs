using System;

namespace Iara.DomainModel.RequestDTOs
{
    public class CreateShipClassificationLogRequestDTO
    {
        public int ShipId { get; set; }
        public int ClassificationYear { get; set; }
        public decimal TotalEngineHours { get; set; }
        public string ClassificationLevel { get; set; }
        public DateTime? ClassificationDate { get; set; }
    }
}