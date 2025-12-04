using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Iara.DomainModel.Entities
{
    [Table("fishing_log_entries")]
    public class FishingLogEntry
    {
        [Key]
        [Column("log_entry_id")]
        public long LogEntryId { get; set; }

        [Required]
        [Column("ship_id")]
        public int ShipId { get; set; }

        [Required]
        [Column("log_date")]
        public DateTime LogDate { get; set; }

        [Column("start_time")]
        public TimeSpan? StartTime { get; set; }

        [Column("end_time")]
        public TimeSpan? EndTime { get; set; }

        [MaxLength(50)]
        [Column("fishing_zone")]
        public string FishingZone { get; set; }

        [Column("catch_details")]
        public string CatchDetails { get; set; }

        [Column("route_details")]
        public string RouteDetails { get; set; }

        [Column("is_signed")]
        public bool IsSigned { get; set; } = false;

        [Column("submitted_at")]
        public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        [ForeignKey("ShipId")]
        public virtual FishingShip Ship { get; set; }
        
        public virtual ICollection<CatchComposition> CatchCompositions { get; set; }
    }
}