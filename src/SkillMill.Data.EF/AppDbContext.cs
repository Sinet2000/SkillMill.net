using Microsoft.EntityFrameworkCore;
using SkillMill.Domain.Entities;

namespace SkillMill.Data.EF;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{

    public DbSet<Customer> Customers => Set<Customer>();

    public DbSet<Order> Orders => Set<Order>();

    public DbSet<Product> Products => Set<Product>();

    public DbSet<OrderItem> OrderItems => Set<OrderItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>()
            .HasMany(c => c.Orders)
            .WithOne(o => o.Customer)
            .HasForeignKey(o => o.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Order>()
            .HasMany(c => c.OrderItems)
            .WithOne(o => o.Order)
            .HasForeignKey(o => o.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<OrderItem>()
            .HasOne(oi => oi.Product)
            .WithMany(p => p.OrderItems)
            .HasForeignKey(oi => oi.ProductId)
            .HasPrincipalKey(p => p.Id);

        modelBuilder.Entity<Product>()
            .HasMany(c => c.OrderItems)
            .WithOne(o => o.Product)
            .HasForeignKey(o => o.ProductId);
    }
}