using Microsoft.EntityFrameworkCore;
using FoodRegistrationTool.Models;

namespace FoodRegistrationTool.DAL;

public class ProductRepository : IProductRepository
{
    //-----------------
    // Product Methods
    //-----------------
    private readonly ProductDbContext _db;

    public ProductRepository(ProductDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<Product>> GetAll()
    {
        return await _db.Products.ToListAsync();
    }

    public async Task<Product?> GetProductById(int id)
    {
        return await _db.Products.FindAsync(id);
    }

    public async Task Create(Product product)
    {
        _db.Products.Add(product);
        await _db.SaveChangesAsync();
    }

    public async Task Update(Product product)
    {
        _db.Products.Update(product);
        await _db.SaveChangesAsync();
    }

    public async Task<bool> Delete(int id)
    {
        var product = await _db.Products.FindAsync(id);
        if (product == null)
        {
            return false;
        }

        _db.Products.Remove(product);
        await _db.SaveChangesAsync();
        return true;
    }

    //-----------------
    // Producer Methods
    //-----------------
    public async Task<IEnumerable<Producer>> GetAllProducers()
    {
        return await _db.Producers.ToListAsync();
    }
    public async Task<Producer?> GetProducerById(int id)
    {
        return await _db.Producers.FindAsync(id);
    }

    public async Task CreateProducer(Producer producer)
    {
        _db.Producers.Add(producer);
        await _db.SaveChangesAsync();
    }

    public async Task UpdateProducer(Producer producer)
    {
        _db.Producers.Update(producer);
        await _db.SaveChangesAsync();
    }

    // Delete Producer method
    public async Task<bool> DeleteProducer(int id)
    {
        try
        {
            var producer = await _db.Producers.FindAsync(id);
            if (producer == null)
            {
                // No producer found
                return false;
            }

            var products = await _db.Products.Where(p => p.ProducerId == id).ToListAsync();
            if (products.Count == 0)
            {
                // Producer has no products
                _db.Producers.Remove(producer);
            }
            else
            {
                // Producer has products
                _db.Products.RemoveRange(products);
                _db.Producers.Remove(producer);
            }

            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            // Logg feilen her om nødvendig
            // f.eks. _logger.LogError(ex, "Feil ved sletting av produsent med id {Id}", id);
            return false;
        }
    }
}