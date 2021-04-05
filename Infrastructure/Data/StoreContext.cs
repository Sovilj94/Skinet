
using Microsoft.EntityFrameworkCore;
using Core.Entities;
using System.Reflection;
using System.Linq;

namespace Infrastructure.Data
{
    public class StoreContext : DbContext
    {
        public StoreContext(DbContextOptions<StoreContext> options) : base(options)
        {
            
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductBrand> ProductBrands { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            if(Database.ProviderName == "Microsoft.EntityFrameworkCore.Sqlite")
            {
                foreach(var e in modelBuilder.Model.GetEntityTypes())
                {
                    var properties = e.ClrType.GetProperties().Where(p => p.PropertyType == typeof(decimal));

                    foreach(var p in properties)
                    {
                        modelBuilder.Entity(e.Name).Property(p.Name).HasConversion<double>();
                    }
                }
            }
        }
        
    }
}