using Microsoft.EntityFrameworkCore;
using FoodRegistrationTool.Models;
using Microsoft.AspNetCore.Razor.Language;

namespace FoodRegistrationTool.DAL;

public class ProductRepository : IProductRepository
{
    //-----------------
    // Product Methods
    //-----------------
    private readonly ProductDbContext _db;
    private readonly ILogger<ProductRepository> _logger;

    public ProductRepository(ProductDbContext db, ILogger<ProductRepository> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task<IEnumerable<Product>?> GetAll()
    {
        try
        {
            return await _db.Products.ToListAsync();
        }
        catch (Exception e)
        {
            _logger.LogError("[ProductRepository] items ToListAsync() failed when GetAll(), error message: {e}", e.Message);
            return null;
        }
    }

    public async Task<Product?> GetProductById(int id)
    {
        try
        {
            return await _db.Products.FindAsync(id);
        }
        catch (Exception e)
        {
            _logger.LogError("[ProductRepository] item FindAsync(id) failed when GetProductById for ProductId {ProductId:0000}, error message: {e}", id, e.Message);
            return null;
        }

    }

    public async Task<bool> Create(Product product)
    {
        try
        {
            _db.Products.Add(product);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("[ProductRepository] item creation failed for product {@product}, error message: {e}", product, e.Message);
            return false;
        }

    }

    public async Task<bool> Update(Product product)
    {
        try
        {
            _db.Products.Update(product);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("[ProductRepository] product FindAsync(id) failed when updating the ProductId {ProductId:0000}, error message: {e}", product, e.Message);
            return false;
        }

    }

    public async Task<bool> Delete(int id)
    {
        try
        {
            var product = await _db.Products.FindAsync(id);
            if (product == null)
            {
                _logger.LogError("[ProductRepository] product not found for the ProductId {ProductId:0000}", id);
                return false;
            }

            _db.Products.Remove(product);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("[ProductRepository] product deletion failed for the ProductId {ProductId:0000}, error message: {e}", id, e.Message);
            return false;
        }

    }

    //-----------------
    // Producer Methods
    //-----------------
    public async Task<IEnumerable<Producer>> GetAllProducers()
    {
        try
        {
            return await _db.Producers.ToListAsync();
        }
        catch (Exception e)
        {
            _logger.LogError("[ProductRepository] producers ToListAsync() failed when GetAllProducers(), error message: {e}", e.Message);
            return Enumerable.Empty<Producer>(); // Returnerer en tom liste ved feil
        }
    }

    public async Task<Producer?> GetProducerById(int id)
    {
        try
        {
            return await _db.Producers.FindAsync(id);
        }
        catch (Exception e)
        {
            _logger.LogError("[ProductRepository] producer FindAsync(id) failed when GetProducerById for ProducerId {ProducerId:0000}, error message: {e}", id, e.Message);
            return null; // Returnerer null ved feil
        }
    }

    public async Task<bool> CreateProducer(Producer producer)
    {
        try
        {
            _db.Producers.Add(producer);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("[ProductRepository] producer SaveChangesAsync() creation failed for producer {@producer}, error message: {e}", producer, e.Message);
            return false;
        }
    }

    public async Task<bool> UpdateProducer(Producer producer)
    {
        try
        {
            _db.Producers.Update(producer);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("[ProductRepository] producer SaveChangesAsync() failed when updating the Producer {@producer}, error message: {e}", producer, e.Message);
            return false;
        }
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
                _logger.LogError("[ProductRepository] producer not found for the ProducerId {ProducerId:0000}", id);
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
            _logger.LogError("[ProductRepository] producer deletion failed for the ProducerId {ProducerId:0000}, error message: {e}", id, e.Message);
            return false;
        }
    }
}