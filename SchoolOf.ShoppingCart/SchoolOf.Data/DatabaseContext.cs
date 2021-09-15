using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SchoolOf.Common;
using SchoolOf.Data.Models;
using SchoolOf.Data.Seed;
using System;

namespace SchoolOf.Data
{
    public class DatabaseContext : DbContext
    {
        private readonly DbSettings _dbSettings;
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Cart> Carts { get; set; }

        public DatabaseContext(IOptions<DbSettings> optionSettings)
        {
            this._dbSettings = optionSettings.Value;
        }
      

        protected override void OnConfiguring(DbContextOptionsBuilder optionBuilder)
        {
            optionBuilder.UseSqlServer(this._dbSettings.ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasKey(x => x.Id);
            modelBuilder.Entity<Product>().Property(x => x.Name).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<Product>().Property(x => x.Description).IsRequired().HasMaxLength(1000);
            modelBuilder.Entity<Product>().Property(x => x.Price).IsRequired();
            modelBuilder.Entity<Product>().Property(x => x.Image).IsRequired();
            modelBuilder.Entity<Product>().Property(x => x.Category).IsRequired().HasMaxLength(100);
          
            modelBuilder.Entity<Product>().HasData(ProductInitialData.GetData());
            OnOrderCreating(modelBuilder);
            OnCartCreating(modelBuilder);
        }

       
        private void OnOrderCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>().Property(x => x.Name).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<Order>().Property(x => x.Address).IsRequired().HasMaxLength(500);
            modelBuilder.Entity<Order>().Property(x => x.Email).IsRequired().HasMaxLength(255); 
            modelBuilder.Entity<Order>().Property(x => x.PhoneNo).IsRequired().HasMaxLength(10); 
            modelBuilder.Entity<Order>().Property(x => x.Total).IsRequired();
        }
        private void OnCartCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cart>().Property(x => x.Status).IsRequired();
        }
    }
}
