using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalog.API.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.API.Infrastructure.EntityConfigurations
{
    public class GenresEntityTypeConfiguration : IEntityTypeConfiguration<Genres>
    {
        public void Configure(EntityTypeBuilder<Genres> builder)
        {
            builder.ToTable("Genres");

            builder.HasKey(ci => ci.Id);

            builder.Property(ci => ci.Id).UseHiLo("genres_hilo");

            builder.Property(ci => ci.Type);
        }
    }
}
