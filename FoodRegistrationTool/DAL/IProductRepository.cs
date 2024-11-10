using FoodRegistrationTool.Models;

namespace FoodRegistrationTool.DAL;

public interface IProductRepository
{
    // Product CRUD
    Task<IEnumerable<Product>> GetAll();
    Task<Product?> GetProductById(int id);
    Task Create(Product product);
    Task Update(Product product);
    Task<bool> Delete(int id);

    // Producer CRUD
    Task<IEnumerable<Producer>> GetAllProducers();
    Task<Producer?> GetProducerById(int id);
    Task<bool> CreateProducer(Producer producer);
    Task<bool> UpdateProducer(Producer producer);
    Task<bool> DeleteProducer(int id);
}