namespace Iara.DomainModel.Filters
{
    public class FishingShipFilter
    {
        public string IaraIdNumber { get; set; }
        public string MaritimeNumber { get; set; }
        public string ShipName { get; set; }
        public string OwnerName { get; set; }
        public bool? IsActive { get; set; }
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
    }
}