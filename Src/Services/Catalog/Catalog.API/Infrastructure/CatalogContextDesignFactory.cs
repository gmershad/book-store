using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Catalog.API.Infrastructure
{
    public class CatalogContextDesignFactory : IDesignTimeDbContextFactory<CatalogContext>
    {
        public CatalogContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CatalogContext>().UseNpgsql
                 ("Host=localhost;Port=5432;Database=BookCatalogDb;UserName=postgres;Password=Philips!");

            return new CatalogContext(optionsBuilder.Options);
        }
    }
}
