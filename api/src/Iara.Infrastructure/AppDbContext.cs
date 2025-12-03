using Iara.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Iara.Infrastructure;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Vessel> Vessels => Set<Vessel>();
    public DbSet<Species> Species => Set<Species>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Vessel>(b =>
        {
            b.ToTable("vessels");
            b.HasKey(x => x.VesselId);
            b.Property(x => x.VesselId).HasColumnName("vessel_id");
            b.Property(x => x.RegNumber).HasColumnName("reg_number").HasMaxLength(50);
            b.Property(x => x.VesselName).HasColumnName("vessel_name").HasMaxLength(100);
        });

        modelBuilder.Entity<Species>(b =>
        {
            b.ToTable("species");
            b.HasKey(x => x.SpeciesId);
            b.Property(x => x.SpeciesId).HasColumnName("species_id");
            b.Property(x => x.SpeciesName).HasColumnName("species_name").HasMaxLength(100);
        });
    }
}
