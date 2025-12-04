using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Iara.Domain.Entities;

namespace Iara.Infrastructure.Persistence.Configurations;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("refresh_tokens");

        builder.HasKey(rt => rt.TokenId);
        builder.Property(rt => rt.TokenId).HasColumnName("token_id");

        builder.Property(rt => rt.UserId)
            .IsRequired()
            .HasColumnName("user_id");

        builder.Property(rt => rt.Token)
            .IsRequired()
            .HasMaxLength(500)
            .HasColumnName("token");

        builder.HasIndex(rt => rt.Token).IsUnique();

        builder.Property(rt => rt.ExpiresAt)
            .IsRequired()
            .HasColumnName("expires_at");

        builder.Property(rt => rt.IsRevoked)
            .IsRequired()
            .HasDefaultValue(false)
            .HasColumnName("is_revoked");

        builder.Property(rt => rt.RevokedByIp)
            .HasMaxLength(50)
            .HasColumnName("revoked_by_ip");

        builder.Property(rt => rt.RevokedAt)
            .HasColumnName("revoked_at");

        builder.Property(rt => rt.ReplacedByToken)
            .HasMaxLength(500)
            .HasColumnName("replaced_by_token");

        builder.Property(rt => rt.CreatedByIp)
            .IsRequired()
            .HasMaxLength(50)
            .HasColumnName("created_by_ip");

        builder.Property(rt => rt.CreatedAt)
            .IsRequired()
            .HasColumnName("created_at");

        builder.Property(rt => rt.UpdatedAt)
            .HasColumnName("updated_at");

        // Foreign key relationship
        builder.HasOne(rt => rt.User)
            .WithMany(u => u.RefreshTokens)
            .HasForeignKey(rt => rt.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
