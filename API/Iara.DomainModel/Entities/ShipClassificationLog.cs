using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Iara.DomainModel.Entities
{
    [Table("ship_classification_log")]
    public class ShipClassificationLog
    {
        [Key]
        [Column("log_id")]
        public int LogId { get; set; }

        [Required]
        [Column("ship_id")]
        public int ShipId { get; set; }

        [Required]
        [Column("classification_year")]
        public int ClassificationYear { get; set; }

        [Required]
        [Column("total_engine_hours", TypeName = "numeric(10,2)")]
        public decimal TotalEngineHours { get; set; }

        [MaxLength(50)]
        [Column("classification_level")]
        public string ClassificationLevel { get; set; }

        [Column("classification_date")]
        public DateTime ClassificationDate { get; set; } = DateTime.Today;

        // Navigation properties
        [ForeignKey("ShipId")]
        public virtual FishingShip Ship { get; set; }
    }
}