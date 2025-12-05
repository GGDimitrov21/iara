using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Iara.Domain.Entities;

namespace Iara.Infrastructure.Persistence.Configurations;

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
