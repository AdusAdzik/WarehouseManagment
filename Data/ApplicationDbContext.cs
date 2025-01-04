using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WarehouseManagementSystem.Models;

namespace WarehouseManagment.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<WarehouseManagementSystem.Models.Warehouse> Warehouses { get; set; }
        public DbSet<WarehouseLog> WarehouseLogs { get; set; }

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductLog> ProductLogs { get; set; }
        public DbSet<WarehouseInventory> WarehouseInventories { get; set; }
        public DbSet<WarehouseEvent> WarehouseEvents { get; set; }

        public DbSet<Subcontractor> Subcontractors { get; set; }
        public DbSet<SubcontractorLog> SubcontractorLogs { get; set; } // Add this DbSet
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Always call the base OnModelCreating first
            base.OnModelCreating(modelBuilder);

            // Configure Warehouse/WarehouseLog relationship
            modelBuilder.Entity<Warehouse>()
                .HasMany(w => w.WarehouseLogs)
                .WithOne(log => log.Warehouse)
                .HasForeignKey(log => log.WarehouseId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relationship between Product and IdentityUser for CreatedBy
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Creator)
                .WithMany()
                .HasForeignKey(p => p.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            //  Product/ProductLog relationship
            modelBuilder.Entity<Product>()
                .HasMany(p => p.ProductLogs)
                .WithOne(log => log.Product)
                .HasForeignKey(log => log.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            // Define relationships for WarehouseInventory
            modelBuilder.Entity<WarehouseInventory>()
                .HasOne(wi => wi.Warehouse)
                .WithMany(w => w.WarehouseInventories)
                .HasForeignKey(wi => wi.WarehouseId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascading deletes

            modelBuilder.Entity<WarehouseInventory>()
                .HasOne(wi => wi.Product)
                .WithMany(p => p.WarehouseInventories)
                .HasForeignKey(wi => wi.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            // relationships  WarehouseEvent
            modelBuilder.Entity<WarehouseEvent>()
                .HasOne(we => we.Warehouse)
                .WithMany(w => w.WarehouseEvents)
                .HasForeignKey(we => we.WarehouseId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<WarehouseEvent>()
                .HasOne(we => we.Product)
                .WithMany(p => p.WarehouseEvents)
                .HasForeignKey(we => we.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<WarehouseEvent>()
                .HasOne(we => we.User)
                .WithMany()
                .HasForeignKey(we => we.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Subcontractor-User relationship
            modelBuilder.Entity<Subcontractor>()
                .HasOne(s => s.User)
                .WithMany()
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Subcontractor-Log relationship
            modelBuilder.Entity<Subcontractor>()
                .HasMany(s => s.SubcontractorLogs)
                .WithOne(log => log.Subcontractor)
                .HasForeignKey(log => log.SubcontractorId)
                .OnDelete(DeleteBehavior.Cascade);

            // Subcontractor-WarehouseEvent relationship
            modelBuilder.Entity<WarehouseEvent>()
                .HasOne(we => we.Subcontractor)
                .WithMany(s => s.WarehouseEvents)
                .HasForeignKey(we => we.SubcontractorId)
                .OnDelete(DeleteBehavior.Restrict);


        }
        public DbSet<WarehouseManagementSystem.Models.Subcontractor> Subcontractor { get; set; } = default!;
    }
}
