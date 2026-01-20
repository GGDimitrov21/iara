using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Iara.Domain.Entities;

namespace Iara.Infrastructure.Persistence.Configurations;

public class TicketConfiguration : IEntityTypeConfiguration<Ticket>
{
    public void Configure(EntityTypeBuilder<Ticket> builder)
    {
        builder.ToTable("TICKETS");

        builder.HasKey(t => t.TicketId);
        builder.Property(t => t.TicketId)
            .HasColumnName("ticket_id")
            .UseIdentityColumn();

        builder.Property(t => t.TicketNumber)
            .HasColumnName("ticket_number")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(t => t.ExpiryDate)
            .HasColumnName("expiry_date")
            .HasColumnType("date");

        builder.Property(t => t.PersonStatus)
            .HasColumnName("person_status")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(t => t.IsValidated)
            .HasColumnName("is_validated")
            .HasDefaultValue(false);

        builder.Property(t => t.ValidationDate)
            .HasColumnName("validation_date")
            .HasColumnType("datetime");

        builder.Property(t => t.InspectionId)
            .HasColumnName("inspection_id");

        builder.HasIndex(t => t.TicketNumber).IsUnique();
        builder.HasIndex(t => t.ValidationDate);
        builder.HasIndex(t => t.InspectionId);

        builder.HasOne(t => t.Inspection)
            .WithMany(i => i.Tickets)
            .HasForeignKey(t => t.InspectionId)
            .OnDelete(DeleteBehavior.SetNull);

        // Ignore BaseEntity properties for this table
        builder.Ignore(t => t.CreatedAt);
        builder.Ignore(t => t.UpdatedAt);
    }
}
