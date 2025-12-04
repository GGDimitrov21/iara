using Microsoft.EntityFrameworkCore;
using Iara.Domain.Entities;

namespace Iara.Infrastructure.Persistence;

/// <summary>
/// Database context for IARA fishing management system
/// </summary>
public class IaraDbContext : DbContext
{
    public IaraDbContext(DbContextOptions<IaraDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<FishingShip> FishingShips => Set<FishingShip>();
    public DbSet<Registration> Registrations => Set<Registration>();
    public DbSet<FishingPermit> FishingPermits => Set<FishingPermit>();
    public DbSet<FishingLogEntry> FishingLogEntries => Set<FishingLogEntry>();
    public DbSet<CatchComposition> CatchCompositions => Set<CatchComposition>();
    public DbSet<Inspection> Inspections => Set<Inspection>();
    public DbSet<ShipClassificationLog> ShipClassificationLogs => Set<ShipClassificationLog>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure schema
        modelBuilder.HasDefaultSchema("public");

        // Apply all configurations from the assembly
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(IaraDbContext).Assembly);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Update timestamps
        var entries = ChangeTracker.Entries()
            .Where(e => e.Entity is Domain.Common.BaseEntity && 
                       (e.State == EntityState.Added || e.State == EntityState.Modified));

        foreach (var entry in entries)
        {
            var entity = (Domain.Common.BaseEntity)entry.Entity;
            
            if (entry.State == EntityState.Added)
            {
                entity.CreatedAt = DateTime.UtcNow;
            }
            else if (entry.State == EntityState.Modified)
            {
                entity.UpdatedAt = DateTime.UtcNow;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}
