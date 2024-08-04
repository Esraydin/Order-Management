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
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);
            builder.ToTable("Users");

            builder.Property(u => u.Name)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(u => u.FirstName)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(u => u.LastName)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(u => u.Description)
                .HasMaxLength(500);

            builder.Property(u => u.CreatedDate)
                .IsRequired()
                .HasDefaultValueSql("GETDATE()")
                .ValueGeneratedOnAdd();

            builder.Property(c => c.LastUpdateDate).HasDefaultValueSql("GETDATE()").ValueGeneratedOnAdd();


            // Örnek seed data
            builder.HasData(
                new User
                {
                    Id = new Guid("3e7d477c-7b2b-4bd1-b120-89b3f189340e"),
                    Name = "Örnek Kullanıcı",
                    FirstName="Test",
                    LastName="Test",
                    Email="Test@gmail.com",
                    PasswordHash = [],
                    PasswordSalt = [],
                    Description = "Bu bir örnek kullanıcıdır",
                    CreatedDate = DateTime.UtcNow,
                    LastUpdateDate = DateTime.UtcNow
                }
            );
        }
    }
}
