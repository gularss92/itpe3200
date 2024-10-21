using FoodRegistrationTool.Models;
using Microsoft.AspNetCore.Mvc;
using FoodRegistrationTool.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodRegistrationTool.Controllers;

public class ProductController : Controller 
{
    private readonly ProductDbContext _productDbContext;

    public ProductController(ProductDbContext productDbContext)
    {
        _productDbContext = productDbContext;
    }

    public IActionResult Table()
    {
        List<Product> products = _productDbContext.Products.ToList();
        var productsViewModel = new ProductsViewModel(products, "Table");
        return View(productsViewModel);       
    }

    public IActionResult Grid()
    {
        List<Product> products = _productDbContext.Products.ToList();
        var productsViewModel = new ProductsViewModel(products, "Grid");
        return View(productsViewModel);
    }

    public IActionResult Details(int id)
    {
        List<Product> products = _productDbContext.Products.ToList();
        var product = products.FirstOrDefault(i => i.ProductId == id);
        if (product == null)
        {
            return NotFound();
        }
        return View(product);
    }

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