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
        if (producers == null)
        {
            _logger.LogError("[ProducerController] Producer list not found while executing _productRepository.GetAllProducers()");
            return NotFound("Producer list not found");
        }
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
            bool returnOK = await _productRepository.CreateProducer(producer);
            if (returnOK)
            {
                return RedirectToAction(nameof(Table));
            }
        }
        _logger.LogError("[ProducerController] Producer creation failed {@producer}", producer);
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
            _logger.LogError("[ProducerController] Producer not found when updating ProducerId {ProducerId:0000}", id);
            return BadRequest("Producer not found for the ProducerId");
        }
        return View(producer);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Update(Producer producer)
    {
        if (ModelState.IsValid)
        {
            bool returnOK = await _productRepository.UpdateProducer(producer);
            if (returnOK)
            {
                return RedirectToAction(nameof(Table));
            }
        }
        _logger.LogError("[ProducerController] Producer update failed {@producer}", producer);
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
            _logger.LogError("[ProducerController] Producer not found for the PoducerId {ProducerId:0000}", id);
            return BadRequest("Producer not found for the ProducerId");
        }
        return View(producer);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        bool returnOK = await _productRepository.DeleteProducer(id);
        if (!returnOK)
        {
            _logger.LogError("[ProducerController] Producer deletion failed for the ProducerId {ProducerId}", id);
            return BadRequest("Producer deletion failed");
        }
        return RedirectToAction(nameof(Table));
    }
}