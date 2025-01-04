using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WarehouseManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using WarehouseManagment.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WarehouseManagementSystem.Controllers
{
    [Authorize]
    public class WarehousesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public WarehousesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Warehouse
        public async Task<IActionResult> Index()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return View(await _context.Warehouses
                .Where(w => w.UserId == userId)
                .ToListAsync());
        }

        // GET: Warehouse/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var warehouse = await _context.Warehouses
                .Include(w => w.WarehouseLogs)
                .ThenInclude(log => log.User) // Include User for displaying email
                .FirstOrDefaultAsync(w => w.Id == id);

            if (warehouse == null || warehouse.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
            {
                return Unauthorized(); // Restrict access to the owner
            }

            return View(warehouse);
        }


        // GET: Warehouse/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Warehouse/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Address,City,Country,PostalCode,Email,Phone,Capacity,IsActive")] Warehouse warehouse)
        {
            warehouse.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Assign the logged-in user

            if (ModelState.IsValid)
            {
                _context.Add(warehouse);
                await _context.SaveChangesAsync(); // Ensure warehouse is saved first
                await LogAction(warehouse.Id, "Create");
                return RedirectToAction(nameof(Index));
            }
            return View(warehouse);
        }

        // GET: Warehouse/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var warehouse = await _context.Warehouses.FindAsync(id);
            if (warehouse == null || warehouse.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
            {
                return Unauthorized(); // Restrict access to the owner
            }

            return View(warehouse);
        }

        // POST: Warehouse/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Address,City,Country,PostalCode,Email,Phone,Capacity,IsActive")] Warehouse warehouse)
        {
            if (id != warehouse.Id) return NotFound();

            var warehouseInDb = await _context.Warehouses.AsNoTracking().FirstOrDefaultAsync(w => w.Id == id);
            if (warehouseInDb == null || warehouseInDb.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
            {
                return Unauthorized(); // Restrict access to the owner
            }

            warehouse.UserId = warehouseInDb.UserId; // Preserve the UserId

            if (ModelState.IsValid)
            {
                try
                {
                    var changes = $"Original: {warehouseInDb}, Updated: {warehouse}";

                    _context.Update(warehouse);
                    await _context.SaveChangesAsync(); // Save the warehouse first
                    await LogAction(warehouse.Id, "Edit", changes);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WarehouseExists(warehouse.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(warehouse);
        }

        // GET: Warehouse/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var warehouse = await _context.Warehouses.FirstOrDefaultAsync(w => w.Id == id);
            if (warehouse == null || warehouse.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
            {
                return Unauthorized(); // Restrict access to the owner
            }

            return View(warehouse);
        }

        // POST: Warehouse/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var warehouse = await _context.Warehouses
                .Include(w => w.WarehouseLogs) // Include logs to handle them
                .FirstOrDefaultAsync(w => w.Id == id);

            if (warehouse == null || warehouse.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
            {
                return Unauthorized(); // Restrict access to the owner
            }

            // Remove logs explicitly if needed
            _context.WarehouseLogs.RemoveRange(warehouse.WarehouseLogs);

            // Remove the warehouse
            _context.Warehouses.Remove(warehouse);

            // Save changes to the database
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Inventory(int id)
        {
            try
            {
                var warehouseInventory = await _context.WarehouseInventories
                    .Include(wi => wi.Product)
                    .Where(wi => wi.WarehouseId == id)
                    .ToListAsync();

                var warehouseEvents = await _context.WarehouseEvents
                    .Include(we => we.Product)
                    .Include(we => we.User)
                    .Include(we => we.Subcontractor)
                    .Where(we => we.WarehouseId == id)
                    .OrderByDescending(we => we.EventDate)
                    .ToListAsync();

                var subcontractors = await _context.Subcontractors
                    .Where(s => s.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier) && s.DeletedAt == null)
                    .ToListAsync();

                ViewData["WarehouseEvents"] = warehouseEvents;
                ViewData["Subcontractors"] = new SelectList(subcontractors, "Id", "Name");
                ViewData["WarehouseId"] = id;

                return View(warehouseInventory);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Inventory Error: {ex.Message}");
                return View(new List<WarehouseInventory>());
            }
        }


        private bool WarehouseExists(int id)
        {
            return _context.Warehouses.Any(e => e.Id == id);
        }

        // Log CRUD actions
        private async Task LogAction(int warehouseId, string action, string? changes = null)
        {
            var log = new WarehouseLog
            {
                WarehouseId = warehouseId,
                Action = action,
                Changes = changes,
                Timestamp = DateTime.UtcNow,
                UserId = User.FindFirstValue(ClaimTypes.NameIdentifier)
            };

            _context.WarehouseLogs.Add(log);
            await _context.SaveChangesAsync();
        }
    }
}
