using Microsoft.EntityFrameworkCore;
using Iara.DomainModel.Entities;

namespace Iara.Persistence.Data
{
    public class IaraDbContext : DbContext
    {
        public IaraDbContext(DbContextOptions<IaraDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<FishingShip> FishingShips { get; set; }
        public DbSet<Registration> Registrations { get; set; }
        public DbSet<FishingPermit> FishingPermits { get; set; }
        public DbSet<FishingLogEntry> FishingLogEntries { get; set; }
        public DbSet<CatchComposition> CatchCompositions { get; set; }
        public DbSet<Inspection> Inspections { get; set; }
        public DbSet<ShipClassificationLog> ShipClassificationLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User configuration
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.Username).IsUnique();
                entity.HasIndex(e => e.Email).IsUnique();
            });

            // FishingShip configuration
            modelBuilder.Entity<FishingShip>(entity =>
            {
                entity.HasIndex(e => e.IaraIdNumber).IsUnique();
                entity.HasIndex(e => e.MaritimeNumber).IsUnique();

                entity.HasOne(e => e.RegistrationDocument)
                    .WithMany(r => r.FishingShips)
                    .HasForeignKey(e => e.RegistrationDocumentId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // FishingPermit configuration
            modelBuilder.Entity<FishingPermit>(entity =>
            {
                entity.HasOne(e => e.Ship)
                    .WithMany(s => s.FishingPermits)
                    .HasForeignKey(e => e.ShipId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.RegistrationDocument)
                    .WithMany(r => r.FishingPermits)
                    .HasForeignKey(e => e.RegistrationDocumentId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // FishingLogEntry configuration
            modelBuilder.Entity<FishingLogEntry>(entity =>
            {
                entity.HasIndex(e => new { e.ShipId, e.LogDate }).IsUnique();

                entity.HasOne(e => e.Ship)
                    .WithMany(s => s.FishingLogEntries)
                    .HasForeignKey(e => e.ShipId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // CatchComposition configuration
            modelBuilder.Entity<CatchComposition>(entity =>
            {
                entity.HasOne(e => e.LogEntry)
                    .WithMany(l => l.CatchCompositions)
                    .HasForeignKey(e => e.LogEntryId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Inspection configuration
            modelBuilder.Entity<Inspection>(entity =>
            {
                entity.HasIndex(e => e.ProtocolNumber).IsUnique();

                entity.HasOne(e => e.Inspector)
                    .WithMany(u => u.Inspections)
                    .HasForeignKey(e => e.InspectorId)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(e => e.Ship)
                    .WithMany(s => s.Inspections)
                    .HasForeignKey(e => e.ShipId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ShipClassificationLog configuration
            modelBuilder.Entity<ShipClassificationLog>(entity =>
            {
                entity.HasIndex(e => new { e.ShipId, e.ClassificationYear }).IsUnique();

                entity.HasOne(e => e.Ship)
                    .WithMany(s => s.ClassificationLogs)
                    .HasForeignKey(e => e.ShipId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}