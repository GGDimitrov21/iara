using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Iara.Domain.Entities;

namespace Iara.Infrastructure.Persistence.Configurations;

public class CatchQuotaConfiguration : IEntityTypeConfiguration<CatchQuota>
{
    public void Configure(EntityTypeBuilder<CatchQuota> builder)
    {
        builder.ToTable("CATCH_QUOTAS");

        builder.HasKey(q => q.QuotaId);
        builder.Property(q => q.QuotaId)
            .HasColumnName("quota_id")
            .UseIdentityColumn();

        builder.Property(q => q.PermitId)
            .HasColumnName("permit_id")
            .IsRequired();

        builder.Property(q => q.SpeciesId)
            .HasColumnName("species_id")
            .IsRequired();

        builder.Property(q => q.Year)
            .HasColumnName("year")
            .IsRequired();

        builder.Property(q => q.MinCatchKg)
            .HasColumnName("min_catch_kg")
            .HasPrecision(10, 2);

        builder.Property(q => q.AvgCatchKg)
            .HasColumnName("avg_catch_kg")
            .HasPrecision(10, 2);

        builder.Property(q => q.MaxCatchKg)
            .HasColumnName("max_catch_kg")
            .HasPrecision(10, 2)
            .IsRequired();

        builder.Property(q => q.FuelHoursLimit)
            .HasColumnName("fuel_hours_limit");

        builder.HasIndex(q => new { q.PermitId, q.SpeciesId, q.Year }).IsUnique();
        builder.HasIndex(q => q.SpeciesId);

        builder.HasOne(q => q.Permit)
            .WithMany(p => p.CatchQuotas)
            .HasForeignKey(q => q.PermitId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(q => q.Species)
            .WithMany(s => s.CatchQuotas)
            .HasForeignKey(q => q.SpeciesId)
            .OnDelete(DeleteBehavior.Cascade);

        // Ignore BaseEntity properties for this table
        builder.Ignore(q => q.CreatedAt);
        builder.Ignore(q => q.UpdatedAt);
    }
}
