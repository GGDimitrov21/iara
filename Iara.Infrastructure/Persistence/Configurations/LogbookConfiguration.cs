using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Iara.Domain.Entities;

namespace Iara.Infrastructure.Persistence.Configurations;

public class LogbookConfiguration : IEntityTypeConfiguration<Logbook>
{
    public void Configure(EntityTypeBuilder<Logbook> builder)
    {
        builder.ToTable("LOGBOOK");

        builder.HasKey(l => l.LogEntryId);
        builder.Property(l => l.LogEntryId)
            .HasColumnName("log_entry_id")
            .UseIdentityColumn();

        builder.Property(l => l.VesselId)
            .HasColumnName("vessel_id")
            .IsRequired();

        builder.Property(l => l.CaptainId)
            .HasColumnName("captain_id")
            .IsRequired();

        builder.Property(l => l.StartTime)
            .HasColumnName("start_time")
            .HasColumnType("datetime")
            .IsRequired();

        builder.Property(l => l.DurationHours)
            .HasColumnName("duration_hours");

        builder.Property(l => l.Latitude)
            .HasColumnName("latitude")
            .HasPrecision(9, 6);

        builder.Property(l => l.Longitude)
            .HasColumnName("longitude")
            .HasPrecision(9, 6);

        builder.Property(l => l.SpeciesId)
            .HasColumnName("species_id")
            .IsRequired();

        builder.Property(l => l.CatchKg)
            .HasColumnName("catch_kg")
            .HasPrecision(10, 2);

        builder.HasIndex(l => l.CaptainId);
        builder.HasIndex(l => new { l.VesselId, l.StartTime });

        builder.HasOne(l => l.Vessel)
            .WithMany(v => v.LogbookEntries)
            .HasForeignKey(l => l.VesselId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(l => l.Captain)
            .WithMany(p => p.LogbookEntries)
            .HasForeignKey(l => l.CaptainId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(l => l.Species)
            .WithMany(s => s.LogbookEntries)
            .HasForeignKey(l => l.SpeciesId)
            .OnDelete(DeleteBehavior.Cascade);

        // Ignore BaseEntity properties for this table
        builder.Ignore(l => l.CreatedAt);
        builder.Ignore(l => l.UpdatedAt);
    }
}
