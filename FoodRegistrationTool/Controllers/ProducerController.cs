using Microsoft.AspNetCore.Mvc;
using FoodRegistrationTool.Models;
using FoodRegistrationTool.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;


namespace FoodRegistrationTool.Controllers;

public class ProducerController : Controller
{
    private readonly IProductRepository _productRepository;

    public ProducerController(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<IActionResult> Table()
    {
        var producers = await _productRepository.GetAllProducers();
        return View(producers);
    }

    [HttpGet]
    [Authorize]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create(Producer producer)
    {
        if (ModelState.IsValid)
        {
            // Save product to DB
            await _productRepository.CreateProducer(producer);
            return RedirectToAction(nameof(Table));
        }
        return View(producer);
    }

}