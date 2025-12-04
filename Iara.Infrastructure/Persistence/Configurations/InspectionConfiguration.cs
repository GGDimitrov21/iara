using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Iara.Domain.Entities;

namespace Iara.Infrastructure.Persistence.Configurations;

public class InspectionConfiguration : IEntityTypeConfiguration<Inspection>
{
    public void Configure(EntityTypeBuilder<Inspection> builder)
    {
        builder.ToTable("inspections");

        builder.HasKey(i => i.InspectionId);
        builder.Property(i => i.InspectionId).HasColumnName("inspection_id");

        builder.Property(i => i.InspectorId).HasColumnName("inspector_id");
        builder.Property(i => i.ShipId).HasColumnName("ship_id").IsRequired();

        builder.Property(i => i.InspectionDate)
            .HasColumnName("inspection_date")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(i => i.InspectionLocation)
            .HasColumnName("inspection_location")
            .HasMaxLength(255);

        builder.Property(i => i.ProtocolNumber)
            .HasColumnName("protocol_number")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(i => i.HasViolation)
            .HasColumnName("has_violation")
            .IsRequired();

        builder.Property(i => i.ViolationDescription)
            .HasColumnName("violation_description")
            .HasColumnType("text");

        builder.Property(i => i.SanctionsImposed)
            .HasColumnName("sanctions_imposed")
            .HasColumnType("text");

        builder.Property(i => i.ProofOfViolationUrl)
            .HasColumnName("proof_of_violation_url")
            .HasColumnType("text");

        builder.Property(i => i.IsProcessed)
            .HasColumnName("is_processed")
            .HasDefaultValue(false);

        builder.HasIndex(i => i.ProtocolNumber).IsUnique();
    }
}

public class ShipClassificationLogConfiguration : IEntityTypeConfiguration<ShipClassificationLog>
{
    public void Configure(EntityTypeBuilder<ShipClassificationLog> builder)
    {
        builder.ToTable("ship_classification_log");

        builder.HasKey(c => c.LogId);
        builder.Property(c => c.LogId).HasColumnName("log_id");

        builder.Property(c => c.ShipId).HasColumnName("ship_id").IsRequired();
        builder.Property(c => c.ClassificationYear).HasColumnName("classification_year").IsRequired();

        builder.Property(c => c.TotalEngineHours)
            .HasColumnName("total_engine_hours")
            .HasPrecision(10, 2)
            .IsRequired();

        builder.Property(c => c.ClassificationLevel)
            .HasColumnName("classification_level")
            .HasMaxLength(50);

        builder.Property(c => c.ClassificationDate)
            .HasColumnName("classification_date")
            .HasDefaultValueSql("CURRENT_DATE");

        builder.HasIndex(c => new { c.ShipId, c.ClassificationYear }).IsUnique();
    }
}
