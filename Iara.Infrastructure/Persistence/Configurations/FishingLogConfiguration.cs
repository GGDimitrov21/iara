using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Iara.Domain.Entities;

namespace Iara.Infrastructure.Persistence.Configurations;

public class FishingLogEntryConfiguration : IEntityTypeConfiguration<FishingLogEntry>
{
    public void Configure(EntityTypeBuilder<FishingLogEntry> builder)
    {
        builder.ToTable("fishing_log_entries");

        builder.HasKey(l => l.LogEntryId);
        builder.Property(l => l.LogEntryId).HasColumnName("log_entry_id");

        builder.Property(l => l.ShipId).HasColumnName("ship_id").IsRequired();
        builder.Property(l => l.LogDate).HasColumnName("log_date").IsRequired();
        builder.Property(l => l.StartTime).HasColumnName("start_time");
        builder.Property(l => l.EndTime).HasColumnName("end_time");

        builder.Property(l => l.FishingZone)
            .HasColumnName("fishing_zone")
            .HasMaxLength(50);

        builder.Property(l => l.CatchDetails)
            .HasColumnName("catch_details")
            .HasColumnType("text");

        builder.Property(l => l.RouteDetails)
            .HasColumnName("route_details")
            .HasColumnType("text");

        builder.Property(l => l.IsSigned)
            .HasColumnName("is_signed")
            .HasDefaultValue(false);

        builder.Property(l => l.SubmittedAt)
            .HasColumnName("submitted_at")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasIndex(l => new { l.ShipId, l.LogDate }).IsUnique();

        builder.HasMany(l => l.CatchCompositions)
            .WithOne(c => c.LogEntry)
            .HasForeignKey(c => c.LogEntryId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class CatchCompositionConfiguration : IEntityTypeConfiguration<CatchComposition>
{
    public void Configure(EntityTypeBuilder<CatchComposition> builder)
    {
        builder.ToTable("catch_composition");

        builder.HasKey(c => c.CatchId);
        builder.Property(c => c.CatchId).HasColumnName("catch_id");

        builder.Property(c => c.LogEntryId).HasColumnName("log_entry_id").IsRequired();

        builder.Property(c => c.FishSpecies)
            .HasColumnName("fish_species")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(c => c.WeightKg)
            .HasColumnName("weight_kg")
            .HasPrecision(10, 3);

        builder.Property(c => c.Count).HasColumnName("count");

        builder.Property(c => c.Status)
            .HasColumnName("status")
            .HasMaxLength(20);
    }
}
