using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using OrderManagement.Domain.Entities;
using OrderManagement.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Infrastructure.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(o => o.Id);
            builder.ToTable("Orders");

            builder.Property(o => o.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(o => o.CreatedDate)
                .IsRequired()
                .HasDefaultValueSql("GETDATE()")
                .ValueGeneratedOnAdd();

            builder.Property(c => c.LastUpdateDate).HasDefaultValueSql("GETDATE()").ValueGeneratedOnAdd();


            builder.Property(o => o.OrderCount)
                .IsRequired();

            builder.Property(o => o.UnitPrice)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(o => o.OrderStatus)
                .IsRequired()
                .HasColumnType("int");

            builder.HasOne(o => o.Company)
                .WithMany(c => c.Orders)
                .HasForeignKey(o => o.CompanyId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(c => c.User)
            .WithMany(c => c.Orders)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(c => c.Product)
            .WithMany(c => c.Orders)
            .HasForeignKey(c => c.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

            // Örnek seed data
            builder.HasData(
                new Order
                {
                    Id = new Guid("ee423d32-2a02-49c0-a70d-eeaeadfcf5c2"),
                    Name = "Örnek Sipariş",
                    CreatedDate = DateTime.UtcNow,
                    LastUpdateDate = DateTime.UtcNow,
                    OrderCount = 5,
                    UnitPrice = 10.5m,
                    OrderStatus = OrderStatusEnum.Pending,
                    CompanyId = new Guid("9450a960-4f4c-4967-84c7-1e9f5781dd00"),
                    ProductId= new Guid("9e60eb9a-9e7f-4a3d-8bbc-611bd5798a18"),
                    UserId = new Guid("3e7d477c-7b2b-4bd1-b120-89b3f189340e")
                }
            );
        }
    }
}
