using System;

namespace Iara.DomainModel.Filters
{
    public class InspectionFilter
    {
        public int? ShipId { get; set; }
        public int? InspectorId { get; set; }
        public bool? HasViolation { get; set; }
        public bool? IsProcessed { get; set; }
        public DateTime? InspectionDateFrom { get; set; }
        public DateTime? InspectionDateTo { get; set; }
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
    }
}