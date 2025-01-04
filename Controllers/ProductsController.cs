using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WarehouseManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using WarehouseManagment.Data;

namespace WarehouseManagment.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            var products = await _context.Products
                .Include(p => p.Creator) // Eager load the creator
                .Where(p => p.DeletedAt == null) // Exclude soft-deleted products
                .ToListAsync();

            return View(products);
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var product = await _context.Products
                .Include(p => p.Creator) // Eager load the creator
                .Include(p => p.ProductLogs)
                .ThenInclude(log => log.User) // Include user for logging
                .FirstOrDefaultAsync(p => p.Id == id && p.DeletedAt == null);

            if (product == null) return NotFound();

            return View(product);
        }

        // GET: Products/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Number,Name,Unit,Description,CubicVolume")] Product product)
        {
            product.CreatedBy = User.FindFirstValue(ClaimTypes.NameIdentifier);
            product.CreatedAt = DateTime.UtcNow;

            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                await LogAction(product.Id, "Create", $"Created product {product.Name}.");
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id && p.DeletedAt == null);
            if (product == null) return NotFound();

            return View(product);
        }

        // POST: Products/Edit/5
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Number,Name,Unit,Description,CubicVolume")] Product product)
        {
            if (id != product.Id) return NotFound();

            var existingProduct = await _context.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id && p.DeletedAt == null);
            if (existingProduct == null) return NotFound();

            product.CreatedBy = existingProduct.CreatedBy; // Preserve original creator
            product.CreatedAt = existingProduct.CreatedAt; // Preserve creation timestamp

            if (ModelState.IsValid)
            {
                try
                {
                    var changes = $"Updated product {product.Name}.";
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                    await LogAction(product.Id, "Edit", changes);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id && p.DeletedAt == null);
            if (product == null) return NotFound();

            return View(product);
        }

        // POST: Products/DeleteConfirmed/5
        [HttpPost, ActionName("DeleteConfirmed")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Find the product including soft delete check
            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.Id == id && p.DeletedAt == null);

            // Ensure product exists and is not already soft deleted
            if (product == null)
            {
                return RedirectToAction(nameof(Index)); // Redirect to Index if not found
            }

            // Soft delete the product
            product.DeletedAt = DateTime.UtcNow;
            _context.Products.Update(product);

            // Log the deletion
            var changes = $"Soft deleted product {product.Name}.";
            await LogAction(product.Id, "SoftDelete", changes);

            // Save changes
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id && e.DeletedAt == null);
        }

        // Log CRUD actions
        private async Task LogAction(int productId, string action, string? changes = null)
        {
            var log = new ProductLog
            {
                ProductId = productId,
                Action = action,
                Changes = changes,
                Timestamp = DateTime.UtcNow,
                UserId = User.FindFirstValue(ClaimTypes.NameIdentifier)
            };

            _context.ProductLogs.Add(log);
            await _context.SaveChangesAsync();
        }
    }
}
