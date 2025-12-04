using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Iara.DomainModel.Entities
{
    [Table("catch_composition")]
    public class CatchComposition
    {
        [Key]
        [Column("catch_id")]
        public long CatchId { get; set; }

        [Required]
        [Column("log_entry_id")]
        public long LogEntryId { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("fish_species")]
        public string FishSpecies { get; set; }

        [Column("weight_kg", TypeName = "numeric(10,3)")]
        public decimal? WeightKg { get; set; }

        [Column("count")]
        public int? Count { get; set; }

        [MaxLength(20)]
        [Column("status")]
        public string Status { get; set; }

        // Navigation properties
        [ForeignKey("LogEntryId")]
        public virtual FishingLogEntry LogEntry { get; set; }
    }
}