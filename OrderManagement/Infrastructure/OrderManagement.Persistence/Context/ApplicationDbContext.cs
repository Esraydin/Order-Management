using Microsoft.EntityFrameworkCore;
using OrderManagement.Domain.Entities;
using OrderManagement.Infrastructure.Configurations;
using System.Reflection;

namespace OrderManagement.Persistence.Context;

public class ApplicationDbContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Data Source=DESKTOP-MUR3BJ4\\SQLEXPRESS;Initial Catalog=OrderManagementDb;Integrated Security=true;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
    }

    public DbSet<Company> Companies { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductCategory> ProductCategories { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>().HasOne(o => o.Product).WithMany().HasForeignKey(o => o.ProductId).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Order>().HasOne(o => o.User).WithMany().HasForeignKey(o => o.UserId).OnDelete(DeleteBehavior.Restrict);
        RemoveFixups(modelBuilder, typeof(Company));
        RemoveFixups(modelBuilder, typeof(Order));
        RemoveFixups(modelBuilder, typeof(Product));
        RemoveFixups(modelBuilder, typeof(ProductCategory));
        RemoveFixups(modelBuilder, typeof(User));
        modelBuilder.ApplyConfiguration(new CompanyConfiguration());
        modelBuilder.ApplyConfiguration(new OrderConfiguration());
        modelBuilder.ApplyConfiguration(new ProductCategoryConfiguration());
        modelBuilder.ApplyConfiguration(new ProductConfiguration());
        modelBuilder.ApplyConfiguration(new UserConfiguration());

    }

    protected void RemoveFixups(ModelBuilder modelBuilder, Type type)
    {
        foreach (var relationship in modelBuilder.Model.FindEntityType(type)!.GetForeignKeys())
        {
            relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;
        }
    }
}


