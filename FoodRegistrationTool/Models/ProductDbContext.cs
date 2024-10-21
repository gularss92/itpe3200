using Microsoft.EntityFrameworkCore;

namespace FoodRegistrationTool.Models;

public class ProductDbContext : DbContext
{
    public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options)
    {
         // Fjern hvis vi benytter migrasjon
        Database.EnsureCreated();
    }

    public DbSet<Product> Products { get; set; }
}