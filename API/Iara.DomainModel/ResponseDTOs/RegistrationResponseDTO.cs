using System;

namespace Iara.DomainModel.ResponseDTOs
{
    public class RegistrationResponseDTO
    {
        public int RegistrationId { get; set; }
        public string DocumentType { get; set; }
        public string IssuedBy { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidUntil { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}