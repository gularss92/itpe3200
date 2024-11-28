using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using FoodRegistrationTool.Controllers;
using FoodRegistrationTool.DAL;
using FoodRegistrationTool.Models;
using FoodRegistrationTool.ViewModels;
using Microsoft.AspNetCore.Hosting;

namespace FoodRegistrationTool.Test.Controllers;

public class ProductControllerTests
{
    // Positive test - ProductController.Table
    [Fact]
    public async Task Table_ReturnsViewResultWithProducts_WhenProductsExist()
    {
        // arrange
        var producer = new Producer{ ProducerId = 1, Name = "Producer 1", Address = "Test Road"};

        var products = new List<Product>()
        {
            new Product
            {
                ProductId = 1,
                Name = "Kokt Skinke",
                Category = "Solid Foods",
                Nutrition = "Calories: 895kcal, Saturated Fat: 3,7g, Sugar: 2,1g, Salt: 1,3mg, Fibre: g, Protein: 11g, Fruit/Vegetable: %",
                Price = 35,
                Description = "110g",
                ImageUrl = "skinke.jpg",
                NutriScore = "D",
                ProducerId = 1,
                Producer = producer
            },
            new Product
            {
                ProductId = 2,
                Name = "Banan",
                Category = "Fruit",
                Nutrition = "Calories: 500kcal, Saturated Fat: 2g, Sugar: 5,1g, Salt: 6,3mg, Fibre: 7,5g, Protein: 11g, Fruit/Vegetable: %",
                Price = 20,
                Description = "150g",
                ImageUrl = "banan.jpg",
                NutriScore = "B",
                ProducerId = 2,
                Producer = producer
            }
        };
        
        var mockProductRepository = new Mock<IProductRepository>();
        var mockLogger = new Mock<ILogger<ProductController>>();
        var mockHostEnvironment = new Mock<IWebHostEnvironment>();

        mockProductRepository.Setup(repo => repo.GetAll()).ReturnsAsync(products);
        var productController = new ProductController(
            mockProductRepository.Object,
            mockHostEnvironment.Object,
            mockLogger.Object
        );

        // act
        var result = await productController.Table();
    
        // assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var productsViewModel = Assert.IsAssignableFrom<ProductsViewModel>(viewResult.ViewData.Model);
        Assert.Equal(2, productsViewModel.Products.Count());
        Assert.Equal(products, productsViewModel.Products);
    }

    // Negative test - Incomplete filling of form for creating product
    [Fact]
    public async Task Create_ReturnsViewResultWithModel_WhenModelStateIsInvalid()
    {
        // arrange
        var producer = new Producer{ ProducerId = 1, Name = "Producer 1", Address = "Test Road"};

        var testProductViewModel= new ProductCreateViewModel
        {
            Name = "",
            Category = "Solid Foods",
            Nutrition = "Calories: 895kcal, Saturated Fat: 3,7g, Sugar: 2,1g, Salt: 1,3mg, Fibre: g, Protein: 11g, Fruit/Vegetable: %",
            Price = 35,
            Description = "110g",
            NutriScore = "D",
            ProducerId = 1
        };
        var mockProductRepository = new Mock<IProductRepository>();
        var mockLogger = new Mock<ILogger<ProductController>>();
        var mockHostEnvironment = new Mock<IWebHostEnvironment>();
        
        mockProductRepository.Setup(repo => repo.Create(It.IsAny<Product>())).ReturnsAsync(false);

        var productController = new ProductController(
            mockProductRepository.Object,
            mockHostEnvironment.Object,
            mockLogger.Object
        );
        
        productController.ModelState.AddModelError("Name", "The Name field is required.");

        // act
        var result = await productController.Create(testProductViewModel);
        
        // assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var viewItem = Assert.IsAssignableFrom<ProductCreateViewModel>(viewResult.ViewData.Model);
        Assert.Equal(testProductViewModel, viewItem);
    }
}