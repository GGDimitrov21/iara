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

public class FishingPermitConfiguration : IEntityTypeConfiguration<FishingPermit>
{
    public void Configure(EntityTypeBuilder<FishingPermit> builder)
    {
        builder.ToTable("fishing_permits");

        builder.HasKey(p => p.PermitId);
        builder.Property(p => p.PermitId).HasColumnName("permit_id");

        builder.Property(p => p.ShipId).HasColumnName("ship_id").IsRequired();
        builder.Property(p => p.PermitYear).HasColumnName("permit_year").IsRequired();
        builder.Property(p => p.IssueDate).HasColumnName("issue_date").IsRequired();
        builder.Property(p => p.ValidFrom).HasColumnName("valid_from").IsRequired();
        builder.Property(p => p.ValidUntil).HasColumnName("valid_until").IsRequired();

        builder.Property(p => p.CatchQuotaType)
            .HasColumnName("catch_quota_type")
            .HasMaxLength(50);

        builder.Property(p => p.MinAnnualCatch)
            .HasColumnName("min_annual_catch")
            .HasPrecision(10, 2);

        builder.Property(p => p.MaxAnnualCatch)
            .HasColumnName("max_annual_catch")
            .HasPrecision(10, 2);

        builder.Property(p => p.TotalHoursAnnualLimit)
            .HasColumnName("total_hours_annual_limit")
            .HasPrecision(10, 2);

        builder.Property(p => p.Status)
            .HasColumnName("status")
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(p => p.RegistrationDocumentId)
            .HasColumnName("registration_document_id");

        builder.Property(p => p.CreatedAt)
            .HasColumnName("created_at")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasOne(p => p.RegistrationDocument)
            .WithMany(r => r.FishingPermits)
            .HasForeignKey(p => p.RegistrationDocumentId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
