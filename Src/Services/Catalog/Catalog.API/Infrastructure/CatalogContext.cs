using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalog.API.Infrastructure.EntityConfigurations;
using Catalog.API.Model;
using Microsoft.EntityFrameworkCore;

namespace Catalog.API.Infrastructure
{
    public class CatalogContext : DbContext
    {
        public CatalogContext(DbContextOptions<CatalogContext> options) : base(options)
        {
        }

        public DbSet<Author> Authors { get; set; }

        public DbSet<Book> Books { get; set; }

        public DbSet<BookFormat> BookFormats { get; set; }

        public DbSet<Genres> BookTypes { get; set; }

        public DbSet<Language> Languages { get; set; }

        public DbSet<Offer> Offers { get; set; }

        public DbSet<Publisher> Publishers { get; set; }

        public DbSet<Seller> Sellers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new AuthorEntityTypeConfiguration());
            builder.ApplyConfiguration(new BookEntityTypeConfiguration());
            builder.ApplyConfiguration(new BookFormatEntityTypeConfiguration());
            builder.ApplyConfiguration(new GenresEntityTypeConfiguration());
            builder.ApplyConfiguration(new LanguageEntityTypeConfiguration());
            builder.ApplyConfiguration(new OfferEntityTypeConfiguration());
            builder.ApplyConfiguration(new PublisherEntityTypeConfiguration());
            builder.ApplyConfiguration(new SellerEntityTypeConfiguration());
        }
    }
}
