using Microsoft.AspNetCore.Mvc;
using FoodRegistrationTool.Models;
using FoodRegistrationTool.DAL;
using Microsoft.EntityFrameworkCore;

namespace FoodRegistrationTool.Controllers;

public class ProducerController : Controller
{
    private readonly ProductDbContext _ProductDbContext;

    public ProducerController(ProductDbContext productDbContext)
    {
        _ProductDbContext = productDbContext;
    }

    public async Task<IActionResult> Table()
    {
        List<Producer> producers = await _ProductDbContext.Producers.ToListAsync();
        return View(producers);
    }
}