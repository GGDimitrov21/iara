using System;

namespace Iara.DomainModel.ResponseDTOs
{
    public class FishingLogEntryResponseDTO
    {
        public long LogEntryId { get; set; }
        public int ShipId { get; set; }
        public DateTime LogDate { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public string FishingZone { get; set; }
        public string CatchDetails { get; set; }
        public string RouteDetails { get; set; }
        public bool IsSigned { get; set; }
        public DateTime SubmittedAt { get; set; }
    }
}