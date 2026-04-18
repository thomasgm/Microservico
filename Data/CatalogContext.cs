using Microsoft.EntityFrameworkCore;
using Microservico.Models;

namespace Microservico.Data
{
    public class CatalogContext : DbContext
    {
        public CatalogContext(DbContextOptions<CatalogContext> options) : base(options)
        {
        }
        public DbSet<CatalogBrand> CatalogBrands => Set<CatalogBrand>();
        public DbSet<CatalogType> CatalogTypes => Set<CatalogType>();
        public DbSet<CatalogItem> CatalogItems => Set<CatalogItem>();

    }
}
