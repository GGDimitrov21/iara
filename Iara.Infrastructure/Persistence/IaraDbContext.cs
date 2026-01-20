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

    // New entities matching SQL schema
    public DbSet<Personnel> Personnel => Set<Personnel>();
    public DbSet<Vessel> Vessels => Set<Vessel>();
    public DbSet<Permit> Permits => Set<Permit>();
    public DbSet<Species> Species => Set<Species>();
    public DbSet<CatchQuota> CatchQuotas => Set<CatchQuota>();
    public DbSet<Logbook> Logbook => Set<Logbook>();
    public DbSet<Inspection> Inspections => Set<Inspection>();
    public DbSet<Ticket> Tickets => Set<Ticket>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

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
