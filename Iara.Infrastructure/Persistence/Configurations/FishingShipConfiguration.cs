using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Iara.Domain.Entities;

namespace Iara.Infrastructure.Persistence.Configurations;

public class FishingShipConfiguration : IEntityTypeConfiguration<FishingShip>
{
    public void Configure(EntityTypeBuilder<FishingShip> builder)
    {
        builder.ToTable("fishing_ships");

        builder.HasKey(s => s.ShipId);
        builder.Property(s => s.ShipId).HasColumnName("ship_id");

        builder.Property(s => s.IaraIdNumber)
            .HasColumnName("iara_id_number")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(s => s.MaritimeNumber)
            .HasColumnName("maritime_number")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(s => s.ShipName)
            .HasColumnName("ship_name")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(s => s.OwnerName)
            .HasColumnName("owner_name")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(s => s.Tonnage)
            .HasColumnName("tonnage")
            .HasPrecision(10, 2)
            .IsRequired();

        builder.Property(s => s.ShipLength)
            .HasColumnName("ship_length")
            .HasPrecision(10, 2)
            .IsRequired();

        builder.Property(s => s.EnginePower)
            .HasColumnName("engine_power")
            .HasPrecision(10, 2)
            .IsRequired();

        builder.Property(s => s.FuelType)
            .HasColumnName("fuel_type")
            .HasMaxLength(20);

        builder.Property(s => s.RegistrationDocumentId)
            .HasColumnName("registration_document_id");

        builder.Property(s => s.RegistrationDate)
            .HasColumnName("registration_date")
            .IsRequired();

        builder.Property(s => s.IsActive)
            .HasColumnName("is_active")
            .HasDefaultValue(true);

        builder.Property(s => s.LastUpdated)
            .HasColumnName("last_updated")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasIndex(s => s.IaraIdNumber).IsUnique();
        builder.HasIndex(s => s.MaritimeNumber).IsUnique();

        builder.HasOne(s => s.RegistrationDocument)
            .WithMany(r => r.FishingShips)
            .HasForeignKey(s => s.RegistrationDocumentId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(s => s.FishingPermits)
            .WithOne(p => p.Ship)
            .HasForeignKey(p => p.ShipId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(s => s.FishingLogEntries)
            .WithOne(l => l.Ship)
            .HasForeignKey(l => l.ShipId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(s => s.Inspections)
            .WithOne(i => i.Ship)
            .HasForeignKey(i => i.ShipId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(s => s.ClassificationLogs)
            .WithOne(c => c.Ship)
            .HasForeignKey(c => c.ShipId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
