using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Iara.Domain.Entities;

namespace Iara.Infrastructure.Persistence.Configurations;

public class PersonnelConfiguration : IEntityTypeConfiguration<Personnel>
{
    public void Configure(EntityTypeBuilder<Personnel> builder)
    {
        builder.ToTable("PERSONNEL");

        builder.HasKey(p => p.PersonId);
        builder.Property(p => p.PersonId)
            .HasColumnName("person_id")
            .UseIdentityColumn();

        builder.Property(p => p.Name)
            .HasColumnName("name")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(p => p.Role)
            .HasColumnName("role")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(p => p.ContactEmail)
            .HasColumnName("contact_email")
            .HasMaxLength(100);

        builder.Property(p => p.PasswordHash)
            .HasColumnName("password_hash")
            .HasMaxLength(256);

        builder.Property(p => p.IsActive)
            .HasColumnName("is_active")
            .HasDefaultValue(true);

        builder.Property(p => p.CreatedAt)
            .HasColumnName("created_at");

        builder.Property(p => p.UpdatedAt)
            .HasColumnName("updated_at");

        builder.HasIndex(p => p.ContactEmail).IsUnique();
        builder.HasIndex(p => p.Role);
    }
}
