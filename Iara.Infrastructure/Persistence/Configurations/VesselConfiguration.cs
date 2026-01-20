using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Iara.Domain.Entities;

namespace Iara.Infrastructure.Persistence.Configurations;

public class VesselConfiguration : IEntityTypeConfiguration<Vessel>
{
    public void Configure(EntityTypeBuilder<Vessel> builder)
    {
        builder.ToTable("VESSELS");

        builder.HasKey(v => v.VesselId);
        builder.Property(v => v.VesselId)
            .HasColumnName("vessel_id")
            .UseIdentityColumn();

        builder.Property(v => v.RegNumber)
            .HasColumnName("reg_number")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(v => v.VesselName)
            .HasColumnName("vessel_name")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(v => v.OwnerDetails)
            .HasColumnName("owner_details");

        builder.Property(v => v.CaptainId)
            .HasColumnName("captain_id");

        builder.Property(v => v.LengthM)
            .HasColumnName("length_m")
            .HasPrecision(6, 2);

        builder.Property(v => v.WidthM)
            .HasColumnName("width_m")
            .HasPrecision(6, 2);

        builder.Property(v => v.Tonnage)
            .HasColumnName("tonnage")
            .HasPrecision(10, 2);

        builder.Property(v => v.FuelType)
            .HasColumnName("fuel_type")
            .HasMaxLength(50);

        builder.Property(v => v.EnginePowerKw)
            .HasColumnName("engine_power_kw")
            .HasPrecision(10, 2);

        builder.Property(v => v.DisplacementTons)
            .HasColumnName("displacement_tons")
            .HasPrecision(8, 2);

        builder.HasIndex(v => v.RegNumber).IsUnique();
        builder.HasIndex(v => v.VesselName);
        builder.HasIndex(v => v.CaptainId);

        builder.HasOne(v => v.Captain)
            .WithMany(p => p.CaptainedVessels)
            .HasForeignKey(v => v.CaptainId)
            .OnDelete(DeleteBehavior.SetNull);

        // Ignore BaseEntity properties for this table
        builder.Ignore(v => v.CreatedAt);
        builder.Ignore(v => v.UpdatedAt);
    }
}
