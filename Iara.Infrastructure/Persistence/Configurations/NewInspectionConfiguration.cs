using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Iara.Domain.Entities;

namespace Iara.Infrastructure.Persistence.Configurations;

public class NewInspectionConfiguration : IEntityTypeConfiguration<Inspection>
{
    public void Configure(EntityTypeBuilder<Inspection> builder)
    {
        builder.ToTable("INSPECTIONS");

        builder.HasKey(i => i.InspectionId);
        builder.Property(i => i.InspectionId)
            .HasColumnName("inspection_id")
            .UseIdentityColumn();

        builder.Property(i => i.VesselId)
            .HasColumnName("vessel_id")
            .IsRequired();

        builder.Property(i => i.InspectorId)
            .HasColumnName("inspector_id")
            .IsRequired();

        builder.Property(i => i.InspectionDate)
            .HasColumnName("inspection_date")
            .HasColumnType("datetime")
            .IsRequired();

        builder.Property(i => i.IsLegal)
            .HasColumnName("is_legal")
            .IsRequired();

        builder.Property(i => i.Notes)
            .HasColumnName("notes");

        builder.HasIndex(i => i.InspectorId);
        builder.HasIndex(i => new { i.VesselId, i.InspectionDate });

        builder.HasOne(i => i.Vessel)
            .WithMany(v => v.Inspections)
            .HasForeignKey(i => i.VesselId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(i => i.Inspector)
            .WithMany(p => p.Inspections)
            .HasForeignKey(i => i.InspectorId)
            .OnDelete(DeleteBehavior.NoAction);

        // Ignore BaseEntity properties for this table
        builder.Ignore(i => i.CreatedAt);
        builder.Ignore(i => i.UpdatedAt);
    }
}
