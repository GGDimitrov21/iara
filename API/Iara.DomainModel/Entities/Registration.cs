using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Iara.DomainModel.Entities
{
    [Table("registrations")]
    public class Registration
    {
        [Key]
        [Column("registration_id")]
        public int RegistrationId { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("document_type")]
        public string DocumentType { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("issued_by")]
        public string IssuedBy { get; set; }

        [Required]
        [Column("issue_date")]
        public DateTime IssueDate { get; set; }

        [Required]
        [Column("valid_from")]
        public DateTime ValidFrom { get; set; }

        [Required]
        [Column("valid_until")]
        public DateTime ValidUntil { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public virtual ICollection<FishingShip> FishingShips { get; set; }
        public virtual ICollection<FishingPermit> FishingPermits { get; set; }
    }
}