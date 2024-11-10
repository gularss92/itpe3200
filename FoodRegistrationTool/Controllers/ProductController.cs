using FoodRegistrationTool.Models;
using Microsoft.AspNetCore.Mvc;
using FoodRegistrationTool.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using FoodRegistrationTool.DAL;
using System.Drawing.Printing;

namespace FoodRegistrationTool.Controllers;

public class ProductController : Controller
{
    private readonly IProductRepository _productRepository;
    private readonly ILogger<ProductController> _logger;

    // For image upload and viewing
    private readonly IWebHostEnvironment _hostEnvironment;

    public ProductController(IProductRepository productRepository, IWebHostEnvironment hostEnvironment, ILogger<ProductController> logger)
    {
        _productRepository = productRepository;
        _hostEnvironment = hostEnvironment;
        _logger = logger;
    }

    public async Task<IActionResult> Table()
    {
        var products = await _productRepository.GetAll();
        if (products == null)
        {
            _logger.LogError("[ProductController] Product list not found while executing _productRepository.GetAll()");
            return NotFound("Product list not found");
        }
        var productsViewModel = new ProductsViewModel(products, "Table");
        return View(productsViewModel);
    }

    public async Task<IActionResult> Grid()
    {
        var products = await _productRepository.GetAll();
        if (products == null)
        {
            _logger.LogError("[ProductController] Product list not found while executing _productRepository.GetAll()");
            return NotFound("Product list not found");
        }
        var productsViewModel = new ProductsViewModel(products, "Grid");
        return View(productsViewModel);
    }

    public async Task<IActionResult> Details(int id)
    {
        var product = await _productRepository.GetProductById(id);
        if (product == null)
        {
            _logger.LogError("[ProductController] Product not found for the ProductId {ProductId:0000}", id);
            return NotFound("Product not found for the ProductId");
        }
        return View(product);
    }

    [HttpGet]
    [Authorize]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([Bind("ProductId,Name,Category,Nutrition,NutriScore,Price,Description, ProducerId")] ProductCreateViewModel productCreateViewModel)
    {
        // Må legge inn Logger her
        if (ModelState.IsValid)
        {
            // Getting producer by Id
            var producer = await _productRepository.GetProducerById(productCreateViewModel.ProducerId);

            // Checking producer exists
            if (producer == null)
            {
                return BadRequest("Producer not found.");
            }

            // Creating Product object
            var product = new Product
            {
                Name = productCreateViewModel.Name,
                Category = productCreateViewModel.Category,
                Nutrition = productCreateViewModel.Nutrition,
                Price = productCreateViewModel.Price,
                Description = productCreateViewModel.Description,
                NutriScore = productCreateViewModel.NutriScore,
                ProducerId = productCreateViewModel.ProducerId,
                Producer = producer // Sett produsenten til produktet
            };

            // Handle Image Upload
            var imageFile = Request.Form.Files.FirstOrDefault();
            if (imageFile != null)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(imageFile.FileName);
                string extension = Path.GetExtension(imageFile.FileName);
                fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                string path = Path.Combine(wwwRootPath, "images/clientImages", fileName);
                //Console.WriteLine($"Attempting to save file to: {path}");
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await imageFile.CopyToAsync(fileStream);
                }
                product.ImageUrl = "/images/clientImages/" + fileName;
                //Console.Write($"Set url to: {product.ImageUrl}");

            }
            else
            {
                Console.Write($"No img found, url: {product.ImageUrl}, imgfile: {product.ImageFile}");
            }

            // Save product to DB
            await _productRepository.Create(product);
            return RedirectToAction(nameof(Table));
        }
        return View(productCreateViewModel);
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Update(int id)
    {
        var product = await _productRepository.GetProductById(id);
        if (product == null)
        {
            _logger.LogError("[ProductController] Product not found when updating the ProductId {ProductId:0000}", id);
            return BadRequest("Product not found for the ProductId");
        }
        return View(product);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Update(int id, [Bind("ProductId,Name,Category,Nutrition,NutriScore,Price,Description")] Product product)
    {
        // Må legge inn Logger her
        if (id != product.ProductId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                // Handle Image Upload
                var imageFile = Request.Form.Files.FirstOrDefault();
                if (imageFile != null)
                {
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string fileName = Path.GetFileNameWithoutExtension(imageFile.FileName);
                    string extension = Path.GetExtension(imageFile.FileName);
                    fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    string path = Path.Combine(wwwRootPath + "/images/clientImages", fileName);
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(fileStream);
                    }
                    product.ImageUrl = "/images/clientImages/" + fileName;

                    // Delete old image
                    var oldImagePath = Path.Combine(wwwRootPath, "images/clientImages", product.ImageUrl);
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                await _productRepository.Update(product);

            }

            catch (DbUpdateConcurrencyException)
            {
                if (product == null)
                {
                    return NotFound();
                }
                else throw;
            }

            return RedirectToAction(nameof(Table));
        }
        return View(product);
    }


    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Delete(int id)
    {
        var product = await _productRepository.GetProductById(id);
        if (product == null)
        {
            _logger.LogError("[ProductController] Product not found for the ProductId {ProductId:0000}", id);
            return BadRequest("Product not found for the ProductId");
        }
        return View(product);
    }
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        bool returnOK = await _productRepository.Delete(id);
        if (!returnOK)
        {
            _logger.LogError("[ProductController] Product deletion failed for the ProductId {ProductId:0000}", id);
            return BadRequest("Item deletion failed");   
        }
        return RedirectToAction(nameof(Table));
    }

    [HttpPost]
    public async Task<IActionResult> CalculateNutritionScore(string category, int calories, double saturatedFat, double sugar, double salt, double fibre, double protein, int fruitOrVeg)
    {
        var score = CalculateNutrition.CalculateScore(category, calories, saturatedFat, sugar, salt, fibre, protein, fruitOrVeg);
        return Json(score);
    }

    // Hvis ikke benytter DB
    public List<Product> GetProducts()
    {
        var products = new List<Product>();
        var product1 = new Product
        {
            ProductId = 1,
            Name = "Banana",
            Category = "Fruit",
            Price = 30,
            Description = "Yellow fruit.",
            ImageUrl = "/images/banana.jpg"
        };

        var product2 = new Product
        {
            ProductId = 2,
            Name = "Coke",
            Category = "Beverage",
            Price = 20,
            Description = "The original coke.",
            ImageUrl = "/images/coke.jpg"
        };

        var product3 = new Product
        {
            ProductId = 3,
            Name = "Baguette",
            Category = "Pastry",
            Price = 30,
            Description = "French bread.",
            ImageUrl = "/images/baguette.png"
        };

        products.Add(product1);
        products.Add(product2);
        products.Add(product3);
        return products;

    }

}