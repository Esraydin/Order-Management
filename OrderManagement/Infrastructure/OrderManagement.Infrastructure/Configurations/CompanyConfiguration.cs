using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using OrderManagement.Domain.Entities;
namespace OrderManagement.Infrastructure.Configurations
{
    public class CompanyConfiguration : BaseConfiguration<Company>
    {
        protected override void ConfigureImplementation(EntityTypeBuilder<Company> builder)
        {
            builder.ToTable("Companies");

            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(c => c.Description)
                .HasMaxLength(500);

            builder.Property(c => c.UserId)
                .IsRequired();

            builder.Property(c => c.CreatedDate).HasDefaultValueSql("GETDATE()").ValueGeneratedOnAdd();

            builder.Property(c => c.LastUpdateDate).HasDefaultValueSql("GETDATE()").ValueGeneratedOnAdd();


            builder.HasOne(c => c.User)
            .WithMany(c=>c.Companies)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Restrict);
            // Örnek seed data
            builder.HasData
            (
                new Company
                {
                    Id = new Guid("9450a960-4f4c-4967-84c7-1e9f5781dd00"),
                    Name = "Örnek Şirket",
                    CreatedDate = DateTime.UtcNow,
                    LastUpdateDate = DateTime.UtcNow,
                    Description = "Bu bir örnek şirkettir",
                    UserId = new Guid("3e7d477c-7b2b-4bd1-b120-89b3f189340e")
                }
            );
        }
    }
}
