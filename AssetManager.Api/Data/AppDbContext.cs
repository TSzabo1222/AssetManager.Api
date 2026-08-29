using AssetManager.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace AssetManager.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Employee> Employees => Set<Employee>();
        public DbSet<Asset> Assets => Set<Asset>();
        public DbSet<InventoryItem> InventoryItems => Set<InventoryItem>();
        public DbSet<AssignmentHistory> AssignmentHistories => Set<AssignmentHistory>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Egy Employee - sok Asset (1-to-many)
            modelBuilder.Entity<Asset>()
                .HasOne(a => a.AssignedToEmployee)
                .WithMany(e => e.AssignedAssets)
                .HasForeignKey(a => a.AssignedToEmployeeId)
                .OnDelete(DeleteBehavior.SetNull); // ha törlöd az employee-t, az asset ne vesszen el

            // AssignmentHistory kapcsolatok
            modelBuilder.Entity<AssignmentHistory>()
                .HasOne(h => h.Asset)
                .WithMany()
                .HasForeignKey(h => h.AssetId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AssignmentHistory>()
                .HasOne(h => h.Employee)
                .WithMany()
                .HasForeignKey(h => h.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);

            // Sorozatszám legyen egyedi
            modelBuilder.Entity<Asset>()
                .HasIndex(a => a.SerialNumber)
                .IsUnique();
        }
    }
}
