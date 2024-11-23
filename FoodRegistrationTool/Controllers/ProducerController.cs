using Microsoft.AspNetCore.Mvc;
using FoodRegistrationTool.Models;
using FoodRegistrationTool.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace FoodRegistrationTool.Controllers;

public class ProducerController : Controller
{
    private readonly IProductRepository _productRepository;
    private readonly ILogger<ProducerController> _logger;

    public ProducerController(IProductRepository productRepository, ILogger<ProducerController> logger)
    {
        _productRepository = productRepository;
        _logger = logger;
    }

    // Table view
    public async Task<IActionResult> Table()
    {
        var producers = await _productRepository.GetAllProducers();
        return View(producers);
    }

    // Create Producer
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
            // Save producer in DB
            await _productRepository.CreateProducer(producer);
            return RedirectToAction(nameof(Table));
        }
        return View(producer);
    }

    // Update Producer
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Update(int id)
    {
        var producer = await _productRepository.GetProducerById(id);
        if (producer == null)
        {
            return NotFound();
        }
        return View(producer);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Update(Producer producer)
    {
        if (ModelState.IsValid)
        {
            await _productRepository.UpdateProducer(producer);
            return RedirectToAction(nameof(Table));
        }
        return View(producer);
    }

    // Delete Producer
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Delete(int id)
    {
        var producer = await _productRepository.GetProducerById(id);
        if (producer == null)
        {
            return NotFound();
        }
        return View(producer);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _productRepository.DeleteProducer(id);
        return RedirectToAction(nameof(Table));
    }
}