using Microsoft.EntityFrameworkCore;
 
namespace Products_Categories.Models
{
    public class ProductContext : DbContext
    {
        // base() calls the parent class' constructor passing the "options" parameter along
        public ProductContext(DbContextOptions options) : base(options) { }
        public DbSet<Product> Products {get;set;}
        public DbSet<Category> Categories {get;set;}

        public DbSet<Association> Associations {get;set;}
    }
}
