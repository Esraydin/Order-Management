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
    public class ProductCategoryConfiguration : IEntityTypeConfiguration<ProductCategory>
    {
        public void Configure(EntityTypeBuilder<ProductCategory> builder)
        {
            builder.HasKey(pc => pc.Id);
            builder.ToTable("ProductCategories");

            builder.Property(pc => pc.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(pc => pc.Description)
                .HasMaxLength(500);

            builder.Property(pc => pc.CreatedDate)
                .IsRequired()
                .HasDefaultValueSql("GETDATE()")
                .ValueGeneratedOnAdd();

            builder.Property(c => c.LastUpdateDate).HasDefaultValueSql("GETDATE()").ValueGeneratedOnAdd();


            // Örnek seed data
            builder.HasData(
                new ProductCategory
                {
                    Id = new Guid("5e378b2e-0b75-4a4f-b4f1-881419799561"),
                    Name = "Örnek Kategori",
                    Description = "Bu bir örnek kategoridir",
                    CreatedDate = DateTime.UtcNow,
                    LastUpdateDate = DateTime.UtcNow
                }
            );
        }
    }
}
