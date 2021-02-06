using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalog.API.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.API.Infrastructure.EntityConfigurations
{
    public class PublisherEntityTypeConfiguration : IEntityTypeConfiguration<Publisher>
    {
        public void Configure(EntityTypeBuilder<Publisher> builder)
        {
            builder.ToTable("Publisher");

            builder.HasKey(ci => ci.Id);

            builder.Property(ci => ci.Id).UseHiLo("publisher_hilo");

            builder.Property(ci => ci.Name);

            builder.Property(ci => ci.Address);

            builder.Property(ci => ci.Description);
        }
    }
}
