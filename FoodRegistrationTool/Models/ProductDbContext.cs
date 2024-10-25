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
    
    // Lazy loading
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseLazyLoadingProxies();
    }

    // Hvis det legges til nye klasser, kjør følgende i terminal (se async modulen i canvas):
    // dotnet ef migrations add FoodRegistryDbExpanded
    // dotnet ef database update
}