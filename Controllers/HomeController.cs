using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WarehouseManagment.Data;
using WarehouseManagment.Models;

namespace WarehouseManagment.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var dashboardData = new DashboardViewModel
            {
                WarehouseCount = await _context.Warehouses.CountAsync(),
                ProductCount = await _context.Products
                    .Where(p => p.DeletedAt == null) // Exclude soft-deleted products
                    .CountAsync(),
                SubcontractorCount = await _context.Subcontractors
                    .Where(s => s.DeletedAt == null) // Exclude soft-deleted subcontractors
                    .CountAsync(),
                WarehouseEventCount = await _context.WarehouseEvents.CountAsync()
            };

            return View(dashboardData);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
