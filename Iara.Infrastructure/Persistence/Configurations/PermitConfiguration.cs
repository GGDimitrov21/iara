using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Iara.Domain.Entities;

namespace Iara.Infrastructure.Persistence.Configurations;

public class PermitConfiguration : IEntityTypeConfiguration<Permit>
{
    public void Configure(EntityTypeBuilder<Permit> builder)
    {
        builder.ToTable("PERMITS");

        builder.HasKey(p => p.PermitId);
        builder.Property(p => p.PermitId)
            .HasColumnName("permit_id")
            .UseIdentityColumn();

        builder.Property(p => p.VesselId)
            .HasColumnName("vessel_id")
            .IsRequired();

        builder.Property(p => p.IssueDate)
            .HasColumnName("issue_date")
            .HasColumnType("date")
            .IsRequired();

        builder.Property(p => p.ExpiryDate)
            .HasColumnName("expiry_date")
            .HasColumnType("date")
            .IsRequired();

        builder.Property(p => p.IsActive)
            .HasColumnName("is_active")
            .HasDefaultValue(true);

        builder.HasIndex(p => p.VesselId);
        builder.HasIndex(p => new { p.IssueDate, p.ExpiryDate });

        builder.HasOne(p => p.Vessel)
            .WithMany(v => v.Permits)
            .HasForeignKey(p => p.VesselId)
            .OnDelete(DeleteBehavior.Cascade);

        // Ignore BaseEntity properties for this table
        builder.Ignore(p => p.CreatedAt);
        builder.Ignore(p => p.UpdatedAt);
    }
}
