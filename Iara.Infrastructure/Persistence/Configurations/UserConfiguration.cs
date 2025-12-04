using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Iara.Domain.Entities;

namespace Iara.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder.HasKey(u => u.UserId);
        builder.Property(u => u.UserId).HasColumnName("user_id");

        builder.Property(u => u.Username)
            .HasColumnName("username")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(u => u.PasswordHash)
            .HasColumnName("password_hash")
            .HasMaxLength(60)
            .IsRequired();

        builder.Property(u => u.FullName)
            .HasColumnName("full_name")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(u => u.Email)
            .HasColumnName("email")
            .HasMaxLength(100);

        builder.Property(u => u.Role)
            .HasColumnName("role")
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(u => u.IsActive)
            .HasColumnName("is_active")
            .HasDefaultValue(true);

        builder.Property(u => u.CreatedAt)
            .HasColumnName("created_at")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasIndex(u => u.Username).IsUnique();
        builder.HasIndex(u => u.Email).IsUnique();

        builder.HasMany(u => u.Inspections)
            .WithOne(i => i.Inspector)
            .HasForeignKey(i => i.InspectorId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(u => u.RefreshTokens)
            .WithOne(rt => rt.User)
            .HasForeignKey(rt => rt.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
