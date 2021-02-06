using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalog.API.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.API.Infrastructure.EntityConfigurations
{
    public class BookEntityTypeConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.ToTable("Book");

            builder.HasKey(ci => ci.Id);

            builder.Property(ci => ci.Id).UseHiLo("book_hilo");

            builder.Property(ci => ci.Name);

            builder.Property(ci => ci.CountryOfOrigin);

            builder.Property(ci => ci.Price);

            builder.Property(ci => ci.NoOfPages);

            builder.Property(ci => ci.AvgCustomerRating);

            builder.Property(ci => ci.Description);

            builder.Property(ci => ci.Dimension);

            builder.Property(ci => ci.ISBN10No);

            builder.Property(ci => ci.ISBN13No);

            builder.Property(ci => ci.PublicationDate);

            builder.HasOne(ci => ci.Author)
                .WithMany()
                .HasForeignKey(ci => ci.AuthorId);

            builder.HasOne(ci => ci.BookFormat)
                .WithMany().HasForeignKey(ci => ci.BookFormatId);

            builder.HasOne(ci => ci.Genres).WithMany()
                .HasForeignKey(ci => ci.GenresId);

            builder.HasOne(ci => ci.Langauge)
                .WithMany().HasForeignKey(ci => ci.LanguageId);

            builder.HasOne(ci => ci.Publisher)
                .WithMany().HasForeignKey(ci => ci.PublisherId);

            builder.HasOne(ci => ci.Seller)
                .WithMany().HasForeignKey(ci => ci.SellerId);
        }
    }
}
