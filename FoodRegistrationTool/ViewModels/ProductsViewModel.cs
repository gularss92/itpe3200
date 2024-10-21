using FoodRegistrationTool.Models;

namespace FoodRegistrationTool.ViewModels
{
    public class ProductsViewModel
    {
        public IEnumerable<Product> Products;
        public string? CurrentViewName;

        public ProductsViewModel(IEnumerable<Product> products, string? currentViewName)
        {
            Products = products;
            CurrentViewName = currentViewName;
        }
    }
}