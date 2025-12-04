using System;

namespace Iara.DomainModel.ResponseDTOs
{
    public class InspectionResponseDTO
    {
        public int InspectionId { get; set; }
        public int? InspectorId { get; set; }
        public int ShipId { get; set; }
        public DateTime InspectionDate { get; set; }
        public string InspectionLocation { get; set; }
        public string ProtocolNumber { get; set; }
        public bool HasViolation { get; set; }
        public string ViolationDescription { get; set; }
        public string SanctionsImposed { get; set; }
        public string ProofOfViolationUrl { get; set; }
        public bool IsProcessed { get; set; }
    }
}