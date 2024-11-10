using Microsoft.EntityFrameworkCore;
using FoodRegistrationTool.Models;
using Microsoft.AspNetCore.Razor.Language;

namespace FoodRegistrationTool.DAL;

public class ProductRepository : IProductRepository
{
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

    // Producer Methods
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

}