using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Iara.DomainModel.Entities
{
    [Table("fishing_permits")]
    public class FishingPermit
    {
        [Key]
        [Column("permit_id")]
        public int PermitId { get; set; }

        [Required]
        [Column("ship_id")]
        public int ShipId { get; set; }

        [Required]
        [Column("permit_year")]
        public int PermitYear { get; set; }

        [Required]
        [Column("issue_date")]
        public DateTime IssueDate { get; set; }

        [Required]
        [Column("valid_from")]
        public DateTime ValidFrom { get; set; }

        [Required]
        [Column("valid_until")]
        public DateTime ValidUntil { get; set; }

        [MaxLength(50)]
        [Column("catch_quota_type")]
        public string CatchQuotaType { get; set; }

        [Column("min_annual_catch", TypeName = "numeric(10,2)")]
        public decimal? MinAnnualCatch { get; set; }

        [Column("max_annual_catch", TypeName = "numeric(10,2)")]
        public decimal? MaxAnnualCatch { get; set; }

        [Column("total_hours_annual_limit", TypeName = "numeric(10,2)")]
        public decimal? TotalHoursAnnualLimit { get; set; }

        [Required]
        [MaxLength(20)]
        [Column("status")]
        public string Status { get; set; }

        [Column("registration_document_id")]
        public int? RegistrationDocumentId { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        [ForeignKey("ShipId")]
        public virtual FishingShip Ship { get; set; }
        
        [ForeignKey("RegistrationDocumentId")]
        public virtual Registration RegistrationDocument { get; set; }
    }
}