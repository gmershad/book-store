using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalog.API.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.API.Infrastructure.EntityConfigurations
{
    public class BookFormatEntityTypeConfiguration : IEntityTypeConfiguration<BookFormat>
    {
        public void Configure(EntityTypeBuilder<BookFormat> builder)
        {
            builder.ToTable("BookFormat");

            builder.HasKey(ci => ci.Id);

            builder.Property(ci => ci.Id).UseHiLo("book_format_hilo");

            builder.Property(ci => ci.Type);
        }
    }
}
