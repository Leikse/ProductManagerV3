using Microsoft.EntityFrameworkCore;
using ProductManager.Models;

namespace ProductManager.Data
{
    class ProductManagerContext : DbContext
    {
        public DbSet<Product> Products { get; set;}
        public DbSet<Category> Categories { get; set;}
        public DbSet<Login> Logins { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=.;Database=ProductManager;Integrated Security=True;TrustServerCertificate=True");
        }
    }
}