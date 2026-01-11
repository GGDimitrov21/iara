using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Iara.Domain.Entities;

namespace Iara.Infrastructure.Persistence.Configurations;

public class RegistrationConfiguration : IEntityTypeConfiguration<Registration>
{
    public void Configure(EntityTypeBuilder<Registration> builder)
    {
        builder.ToTable("registrations");

        builder.HasKey(r => r.RegistrationId);
        builder.Property(r => r.RegistrationId).HasColumnName("registration_id");

        builder.Property(r => r.DocumentType)
            .HasColumnName("document_type")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(r => r.IssuedBy)
            .HasColumnName("issued_by")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(r => r.IssueDate)
            .HasColumnName("issue_date")
            .IsRequired();

        builder.Property(r => r.ValidFrom)
            .HasColumnName("valid_from")
            .IsRequired();

        builder.Property(r => r.ValidUntil)
            .HasColumnName("valid_until")
            .IsRequired();

        builder.Property(r => r.Description)
            .HasColumnName("description")
            .HasColumnType("text");

        builder.Property(r => r.CreatedAt)
            .HasColumnName("created_at")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
    }
}
