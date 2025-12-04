using System;

namespace Iara.DomainModel.RequestDTOs
{
    public class CreateFishingLogEntryRequestDTO
    {
        public int ShipId { get; set; }
        public DateTime LogDate { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public string FishingZone { get; set; }
        public string CatchDetails { get; set; }
        public string RouteDetails { get; set; }
    }
}