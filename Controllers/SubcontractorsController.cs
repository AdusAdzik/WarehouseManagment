using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WarehouseManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using WarehouseManagment.Data;

namespace WarehouseManagment.Controllers
{
    [Authorize]
    public class SubcontractorsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SubcontractorsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Subcontractors
        public async Task<IActionResult> Index()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var subcontractors = await _context.Subcontractor
                .Include(s => s.User)
                .Where(s => s.UserId == userId && s.DeletedAt == null) // Exclude soft-deleted subcontractors
                .ToListAsync();

            return View(subcontractors);
        }

        // GET: Subcontractors/Details/5
        // GET: Subcontractors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var subcontractor = await _context.Subcontractor
                .Include(s => s.SubcontractorLogs)
                .ThenInclude(log => log.User) // Include logs and their associated user
                .Include(s => s.WarehouseEvents) // Include related WarehouseEvents
                .ThenInclude(we => we.Product) // Include product details in WarehouseEvents
                .Include(s => s.WarehouseEvents)
                .ThenInclude(we => we.Warehouse) // Include warehouse details in WarehouseEvents
                .FirstOrDefaultAsync(s => s.Id == id && s.UserId == userId && s.DeletedAt == null); // Restrict access to the owning user

            if (subcontractor == null) return Unauthorized();

            return View(subcontractor);
        }

        // GET: Subcontractors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Subcontractors/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Email,Phone,Description")] Subcontractor subcontractor)
        {
            subcontractor.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Assign the logged-in user

            if (ModelState.IsValid)
            {
                _context.Add(subcontractor);
                await _context.SaveChangesAsync();
                await LogAction(subcontractor.Id, "Create");
                return RedirectToAction(nameof(Index));
            }
            return View(subcontractor);
        }

        // GET: Subcontractors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var subcontractor = await _context.Subcontractor
                .FirstOrDefaultAsync(s => s.Id == id && s.UserId == userId); // Restrict access to the owning user

            if (subcontractor == null) return Unauthorized();

            return View(subcontractor);
        }

        // POST: Subcontractors/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Email,Phone,Description")] Subcontractor subcontractor)
        {
            if (id != subcontractor.Id) return NotFound();

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var originalSubcontractor = await _context.Subcontractor
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == id && s.UserId == userId); // Restrict access to the owning user

            if (originalSubcontractor == null) return Unauthorized();

            subcontractor.UserId = originalSubcontractor.UserId; // Preserve the original user ID

            if (ModelState.IsValid)
            {
                try
                {
                    var changes = $"Original: {originalSubcontractor}, Updated: {subcontractor}";
                    _context.Update(subcontractor);
                    await _context.SaveChangesAsync();
                    await LogAction(subcontractor.Id, "Edit", changes);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SubcontractorExists(subcontractor.Id)) return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(subcontractor);
        }

        // GET: Subcontractors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var subcontractor = await _context.Subcontractor
                .Include(s => s.SubcontractorLogs)
                .FirstOrDefaultAsync(s => s.Id == id && s.UserId == userId); // Restrict access to the owning user

            if (subcontractor == null) return Unauthorized();

            return View(subcontractor);
        }

        // POST: Subcontractors/Delete/5
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var subcontractor = await _context.Subcontractor
                .FirstOrDefaultAsync(s => s.Id == id && s.UserId == userId && s.DeletedAt == null); // Check ownership and not already deleted

            if (subcontractor == null) return Unauthorized();

            // Mark as soft deleted
            subcontractor.DeletedAt = DateTime.UtcNow;
            _context.Subcontractor.Update(subcontractor);

            // Log the soft delete action
            await LogAction(subcontractor.Id, "SoftDelete", $"Soft deleted subcontractor {subcontractor.Name}.");

            // Save changes
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool SubcontractorExists(int id)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return _context.Subcontractor.Any(s => s.Id == id && s.UserId == userId && s.DeletedAt == null); // Check ownership and not soft deleted
        }

        private async Task LogAction(int subcontractorId, string action, string? changes = null)
        {
            var log = new SubcontractorLog
            {
                SubcontractorId = subcontractorId,
                Action = action,
                Changes = changes,
                Timestamp = DateTime.UtcNow,
                UserId = User.FindFirstValue(ClaimTypes.NameIdentifier)
            };

            _context.SubcontractorLogs.Add(log);
            await _context.SaveChangesAsync();
        }
    }
}
