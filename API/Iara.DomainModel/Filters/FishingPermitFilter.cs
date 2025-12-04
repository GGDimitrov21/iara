using System;

namespace Iara.DomainModel.Filters
{
    public class FishingPermitFilter
    {
        public int? ShipId { get; set; }
        public int? PermitYear { get; set; }
        public string Status { get; set; }
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
    }
}