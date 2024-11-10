using FoodRegistrationTool.Models;

namespace FoodRegistrationTool.DAL;

public interface IProductRepository
{
    Task<IEnumerable<Product>?> GetAll();
    Task<Product?> GetProductById(int id);
    Task<bool> Create(Product product);
    Task<bool> Update(Product product);
    Task<bool> Delete(int id);

    // Producer CRUD
    Task<IEnumerable<Producer>> GetAllProducers();
    Task<Producer?> GetProducerById(int id);
    Task<bool> CreateProducer(Producer producer);
    Task<bool> UpdateProducer(Producer producer);
    Task<bool> DeleteProducer(int id);
}