using FoodRegistrationTool.Models;
using Microsoft.AspNetCore.Mvc;
using FoodRegistrationTool.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;

namespace FoodRegistrationTool.Controllers;

public class ProductController : Controller 
{
    private readonly ProductDbContext _productDbContext;

    // For image upload and viewing
    private readonly IWebHostEnvironment _hostEnvironment;

    public ProductController(ProductDbContext productDbContext, IWebHostEnvironment hostEnvironment)
    {
        _productDbContext = productDbContext;
        _hostEnvironment = hostEnvironment;
    }

    public async Task<IActionResult> Table()
    {
        List<Product> products = await _productDbContext.Products.ToListAsync();
        var productsViewModel = new ProductsViewModel(products, "Table");
        return View(productsViewModel);       
    }

    public async Task<IActionResult> Grid()
    {
        List<Product> products = await _productDbContext.Products.ToListAsync();
        var productsViewModel = new ProductsViewModel(products, "Grid");
        return View(productsViewModel);
    }

    public async Task<IActionResult> Details(int id)
    {
        var product = await _productDbContext.Products.FirstOrDefaultAsync(i => i.ProductId == id);
        if (product == null)
        {
            return NotFound();
        }
        return View(product);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create([Bind("ProductId,Name,Category,Nutrition,NutriScore,Price,Description")] Product product)
    {
        if (ModelState.IsValid)
        {
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
            _productDbContext.Products.Add(product);
            await _productDbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Table));
        }
        return View(product);
    }

    [HttpGet]
    public async Task<IActionResult> Update(int id)
    {
        var product = await _productDbContext.Products.FindAsync(id);
        if (product == null)
        {
            return NotFound();
        }
        return View(product);
    }
    
    [HttpPost]
        public async Task<IActionResult> Update(int id, [Bind("ProductId,Name,Category,Nutrition,NutriScore,Price,Description")] Product product)
    {
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

                _productDbContext.Products.Update(product);
                await _productDbContext.SaveChangesAsync();

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
    public async Task<IActionResult> Delete(int id)
    {
        var product = await _productDbContext.Products.FindAsync(id);
        if (product == null)
        {
            return NotFound();
        }
        return View(product);
    }
    [HttpPost]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var product = await _productDbContext.Products.FindAsync(id);
        if (product == null)
        {
            return NotFound();
        }
        _productDbContext.Products.Remove(product);
        await _productDbContext.SaveChangesAsync();
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