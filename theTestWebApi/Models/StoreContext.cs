using Microsoft.EntityFrameworkCore;

namespace theTestWebApi.Models
{
    public class StoreContext : DbContext
    {
        public StoreContext(DbContextOptions<StoreContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasData(
                new Product[]
                {
                new Product { ProductId=1, Name="Soap", Cost=200},
                new Product { ProductId=2, Name="Brush", Cost=400},
                new Product { ProductId=3, Name="Toothpaste", Cost=150}
                });
        }
    }
}
