using System;

namespace Iara.DomainModel.ResponseDTOs
{
    public class ShipClassificationLogResponseDTO
    {
        public int LogId { get; set; }
        public int ShipId { get; set; }
        public int ClassificationYear { get; set; }
        public decimal TotalEngineHours { get; set; }
        public string ClassificationLevel { get; set; }
        public DateTime ClassificationDate { get; set; }
    }
}