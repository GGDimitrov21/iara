using System;

namespace Iara.DomainModel.RequestDTOs
{
    public class CreateInspectionRequestDTO
    {
        public int? InspectorId { get; set; }
        public int ShipId { get; set; }
        public DateTime InspectionDate { get; set; }
        public string InspectionLocation { get; set; }
        public string ProtocolNumber { get; set; }
        public bool HasViolation { get; set; }
        public string ViolationDescription { get; set; }
        public string SanctionsImposed { get; set; }
        public string ProofOfViolationUrl { get; set; }
    }
}