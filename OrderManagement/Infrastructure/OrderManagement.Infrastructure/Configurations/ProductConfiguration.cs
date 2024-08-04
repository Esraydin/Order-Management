using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using OrderManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Infrastructure.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(p => p.Id);
            builder.ToTable("Products");

            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(p => p.Description)
                .HasMaxLength(500);

            builder.Property(p => p.StockCount)
                .IsRequired();

            builder.Property(p => p.Price)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(p => p.CreatedDate)
                .IsRequired()
                .HasDefaultValueSql("GETDATE()")
                .ValueGeneratedOnAdd();

            builder.Property(c => c.LastUpdateDate).HasDefaultValueSql("GETDATE()").ValueGeneratedOnAdd();


            builder.HasOne(p => p.Company)
                .WithMany(x=>x.Products)
                .HasForeignKey(p => p.CompanyId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(p => p.ProductCategory)
                .WithMany(x => x.Products)
                .HasForeignKey(p => p.ProductCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // Örnek seed data
            builder.HasData(
                new Product
                {
                    Id = new Guid("9e60eb9a-9e7f-4a3d-8bbc-611bd5798a18"),
                    Name = "Örnek Ürün",
                    Description = "Bu bir örnek üründür",
                    StockCount = 100,
                    Price = 50.0m,
                    CompanyId = new Guid("9450a960-4f4c-4967-84c7-1e9f5781dd00"),
                    ProductCategoryId = new Guid("5e378b2e-0b75-4a4f-b4f1-881419799561"),
                    CreatedDate = DateTime.UtcNow,
                    LastUpdateDate = DateTime.UtcNow
                }
            );
        }
    }
}
