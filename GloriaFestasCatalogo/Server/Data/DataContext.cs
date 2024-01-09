using GloriaFestasCatalogo.Shared.Models.Config;
using GloriaFestasCatalogo.Shared.Models.Orders;
using GloriaFestasCatalogo.Shared.Models.Products;
using GloriaFestasCatalogo.Shared.Models.Users;
using Microsoft.EntityFrameworkCore;

namespace GloriaFestasCatalogo.Server.Data
{
    public class DataContext : DbContext
    {

        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> Categories { get; set; }
        public DbSet<AppConfig> AppConfig { get; set; }
        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<User>()
                .Property(u => u.Username)
                .IsRequired();

            modelBuilder.Entity<Product>()
                .Property(p => p.Name)
                .IsRequired();
            modelBuilder.Entity<Product>()
                .Property(p => p.Description)
                .IsRequired();
            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<ProductCategory>()
                .Property(c => c.Name)
                .IsRequired();

            modelBuilder.Entity<Order>()
                .Property(o => o.Name)
                .IsRequired();
            modelBuilder.Entity<Order>()
                .Property(o => o.Whatsapp)
                .IsRequired();
            modelBuilder.Entity<Order>()
                .Property(o => o.ZipCode)
                .IsRequired();
            modelBuilder.Entity<Order>()
                .Property(o => o.Street)
                .IsRequired();
            modelBuilder.Entity<Order>()
                .Property(o => o.Neighborhood)
                .IsRequired();
            modelBuilder.Entity<Order>()
                .Property(o => o.City)
                .IsRequired();
            modelBuilder.Entity<Order>()
                .Property(o => o.State)
                .IsRequired();
            modelBuilder.Entity<Order>()
                .Property(o => o.TotalPrice)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            modelBuilder.Entity<Order>()
                .HasMany(o => o.Products)
                .WithOne()
                .HasForeignKey(oc => oc.OrderId);

            modelBuilder.Entity<OrderCart>()
                .HasKey(oc => new { oc.OrderId, oc.ProductId });

            modelBuilder.Entity<OrderCart>()
                .HasOne(oc => oc.Order)
                .WithMany(o => o.Products)
                .HasForeignKey(oc => oc.OrderId);

            modelBuilder.Entity<OrderCart>()
                .HasOne(oc => oc.Product)
                .WithMany()
                .HasForeignKey(oc => oc.ProductId);

            modelBuilder.Entity<AppConfig>()
               .Property(u => u.PhoneNumber)
               .IsRequired();

        }
    }
}
