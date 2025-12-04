using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Iara.DomainModel.Entities
{
    [Table("fishing_ships")]
    public class FishingShip
    {
        [Key]
        [Column("ship_id")]
        public int ShipId { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("iara_id_number")]
        public string IaraIdNumber { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("maritime_number")]
        public string MaritimeNumber { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("ship_name")]
        public string ShipName { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("owner_name")]
        public string OwnerName { get; set; }

        [Required]
        [Column("tonnage", TypeName = "numeric(10,2)")]
        public decimal Tonnage { get; set; }

        [Required]
        [Column("ship_length", TypeName = "numeric(10,2)")]
        public decimal ShipLength { get; set; }

        [Required]
        [Column("engine_power", TypeName = "numeric(10,2)")]
        public decimal EnginePower { get; set; }

        [MaxLength(20)]
        [Column("fuel_type")]
        public string FuelType { get; set; }

        [Column("registration_document_id")]
        public int? RegistrationDocumentId { get; set; }

        [Required]
        [Column("registration_date")]
        public DateTime RegistrationDate { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; } = true;

        [Column("last_updated")]
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;

        // Navigation properties
        [ForeignKey("RegistrationDocumentId")]
        public virtual Registration RegistrationDocument { get; set; }
        
        public virtual ICollection<FishingPermit> FishingPermits { get; set; }
        public virtual ICollection<FishingLogEntry> FishingLogEntries { get; set; }
        public virtual ICollection<Inspection> Inspections { get; set; }
        public virtual ICollection<ShipClassificationLog> ClassificationLogs { get; set; }
    }
}