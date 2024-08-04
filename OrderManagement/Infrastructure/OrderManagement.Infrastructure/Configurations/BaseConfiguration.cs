using OrderManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
namespace OrderManagement.Infrastructure.Configurations
{
    public abstract class BaseConfiguration<T> : IEntityTypeConfiguration<T>
    where T : BaseEntity
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            builder.HasKey(x => x.Id);

            ConfigureImplementation(builder);
        }

        protected abstract void ConfigureImplementation(EntityTypeBuilder<T> builder);
    }

}
