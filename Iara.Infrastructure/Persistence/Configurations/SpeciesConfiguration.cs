using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Iara.Domain.Entities;

namespace Iara.Infrastructure.Persistence.Configurations;

public class SpeciesConfiguration : IEntityTypeConfiguration<Species>
{
    public void Configure(EntityTypeBuilder<Species> builder)
    {
        builder.ToTable("SPECIES");

        builder.HasKey(s => s.SpeciesId);
        builder.Property(s => s.SpeciesId)
            .HasColumnName("species_id")
            .UseIdentityColumn();

        builder.Property(s => s.SpeciesName)
            .HasColumnName("species_name")
            .HasMaxLength(100)
            .IsRequired();

        builder.HasIndex(s => s.SpeciesName).IsUnique();

        // Ignore BaseEntity properties for this table
        builder.Ignore(s => s.CreatedAt);
        builder.Ignore(s => s.UpdatedAt);
    }
}
