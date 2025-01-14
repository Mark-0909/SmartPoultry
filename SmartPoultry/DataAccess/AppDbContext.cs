using Microsoft.EntityFrameworkCore;
using System;
using System.IO;

namespace SmartPoultry.DataAccess
{
    public class AppDbContext : DbContext
    {
        public DbSet<Models.User> Users { get; set; }
        public DbSet<Models.Products> Products { get; set; }
        public DbSet<Models.ProductVariations> ProductVariations { get; set; }
        
        public DbSet<Models.SupplierList> SupplierLists { get; set; }
        public DbSet<Models.Sales> Sales { get; set; }
        
        public DbSet<Models.Deliveries> Deliveries { get; set; }

        public DbSet<Models.FinancialLiabilities> FinancialLiabilities { get; set; }

        public DbSet<Models.UserLogs> UserLogs { get; set; }
        public DbSet<Models.InventoryLogs> InventoryLogs { get; set; }

        public DbSet<Models.SupplierLogs> SupplierLogs { get; set; }

        public DbSet<Models.SupplierOrders> SupplierOrders { get; set; }

        public DbSet<Models.Expenses> Expenses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            string dbFilePath = Path.Combine(folderPath, "SmartPoultryDatabase.db");
            optionsBuilder.UseSqlite($"Data Source={dbFilePath}");

            
        }
    }
}
