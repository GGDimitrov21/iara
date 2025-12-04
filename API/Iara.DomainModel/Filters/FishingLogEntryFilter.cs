using System;

namespace Iara.DomainModel.Filters
{
    public class FishingLogEntryFilter
    {
        public int? ShipId { get; set; }
        public DateTime? LogDateFrom { get; set; }
        public DateTime? LogDateTo { get; set; }
        public string FishingZone { get; set; }
        public bool? IsSigned { get; set; }
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
    }
}