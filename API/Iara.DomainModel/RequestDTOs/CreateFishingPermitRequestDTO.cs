using System;

namespace Iara.DomainModel.RequestDTOs
{
    public class CreateFishingPermitRequestDTO
    {
        public int ShipId { get; set; }
        public int PermitYear { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidUntil { get; set; }
        public string CatchQuotaType { get; set; }
        public decimal? MinAnnualCatch { get; set; }
        public decimal? MaxAnnualCatch { get; set; }
        public decimal? TotalHoursAnnualLimit { get; set; }
        public string Status { get; set; }
        public int? RegistrationDocumentId { get; set; }
    }
}