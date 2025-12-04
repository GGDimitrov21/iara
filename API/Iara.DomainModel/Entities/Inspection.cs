using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Iara.DomainModel.Entities
{
    [Table("inspections")]
    public class Inspection
    {
        [Key]
        [Column("inspection_id")]
        public int InspectionId { get; set; }

        [Column("inspector_id")]
        public int? InspectorId { get; set; }

        [Required]
        [Column("ship_id")]
        public int ShipId { get; set; }

        [Column("inspection_date")]
        public DateTime InspectionDate { get; set; } = DateTime.UtcNow;

        [MaxLength(255)]
        [Column("inspection_location")]
        public string InspectionLocation { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("protocol_number")]
        public string ProtocolNumber { get; set; }

        [Required]
        [Column("has_violation")]
        public bool HasViolation { get; set; }

        [Column("violation_description")]
        public string ViolationDescription { get; set; }

        [Column("sanctions_imposed")]
        public string SanctionsImposed { get; set; }

        [Column("proof_of_violation_url")]
        public string ProofOfViolationUrl { get; set; }

        [Column("is_processed")]
        public bool IsProcessed { get; set; } = false;

        // Navigation properties
        [ForeignKey("InspectorId")]
        public virtual User Inspector { get; set; }
        
        [ForeignKey("ShipId")]
        public virtual FishingShip Ship { get; set; }
    }
}