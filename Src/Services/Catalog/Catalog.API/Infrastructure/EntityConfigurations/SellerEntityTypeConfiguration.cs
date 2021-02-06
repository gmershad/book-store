using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalog.API.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.API.Infrastructure.EntityConfigurations
{
    public class SellerEntityTypeConfiguration : IEntityTypeConfiguration<Seller>
    {
        public void Configure(EntityTypeBuilder<Seller> builder)
        {
            builder.ToTable("Seller");

            builder.HasKey(ci => ci.Id);

            builder.Property(ci => ci.Id).UseHiLo("seller_hilo");

            builder.Property(ci => ci.Name);

            builder.Property(ci => ci.Description);

            builder.Property(ci => ci.AvgRating);
        }
    }
}
