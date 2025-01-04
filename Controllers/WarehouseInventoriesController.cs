using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WarehouseManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using WarehouseManagment.Data;
using WarehouseManagment.Models;

namespace WarehouseManagementSystem.Controllers
{
    [Authorize]
    public class WarehouseInventoriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public WarehouseInventoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Display products for a selected warehouse
        public async Task<IActionResult> ProductsForWarehouse(int warehouseId)
        {
            var warehouse = await _context.Warehouses.FindAsync(warehouseId);
            if (warehouse == null)
            {
                return NotFound();
            }

            // Exclude products that are soft-deleted
            var products = await _context.Products
                .Where(p => p.DeletedAt == null) // Exclude deleted products
                .ToListAsync();
            ViewData["WarehouseId"] = warehouseId;

            return View(products);
        }

        // POST: Add product to cart
        [HttpPost]
        public IActionResult AddToCart(int warehouseId, int productId, double quantity)
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();

            var existingItem = cart.FirstOrDefault(c => c.ProductId == productId && c.WarehouseId == warehouseId);
            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                cart.Add(new CartItem { WarehouseId = warehouseId, ProductId = productId, Quantity = quantity });
            }

            HttpContext.Session.SetObjectAsJson("Cart", cart);
            return Json(new { success = true });
        }

        // POST: Add product to cart whenm issueing from warehouse to subcontractor

        [HttpPost]
        public IActionResult AddToIssueCart(int warehouseId, int productId, double quantity)
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();

            var inventory = _context.WarehouseInventories
                .FirstOrDefault(wi => wi.WarehouseId == warehouseId && wi.ProductId == productId);

            if (inventory == null || inventory.Quantity < quantity)
            {
                return Json(new { success = false, message = "Insufficient stock in inventory." });
            }

            var existingItem = cart.FirstOrDefault(c => c.ProductId == productId && c.WarehouseId == warehouseId);
            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                cart.Add(new CartItem { WarehouseId = warehouseId, ProductId = productId, Quantity = quantity });
            }

            HttpContext.Session.SetObjectAsJson("Cart", cart);
            return Json(new { success = true });
        }


        // GET: Cart Summary (returns JSON)
        [HttpGet]
        public IActionResult CartSummary()
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();

            foreach (var item in cart)
            {
                var product = _context.Products.FirstOrDefault(p => p.Id == item.ProductId);
                if (product != null)
                {
                    item.Number = product.Number;
                    item.Name = product.Name;
                }
            }

            return Json(cart);
        }


        // POST: Remove product from cart
        [HttpPost]
        public IActionResult RemoveFromCart(int productId)
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();
            cart = cart.Where(c => c.ProductId != productId).ToList();
            HttpContext.Session.SetObjectAsJson("Cart", cart);
            return Json(new { success = true });
        }

        // POST: Clear cart
        [HttpPost]
        public IActionResult ClearCart()
        {
            HttpContext.Session.Remove("Cart");
            return Json(new { success = true });
        }
        // POST: Finalize reception
        [HttpPost]
        [HttpPost]
        public async Task<IActionResult> FinalizeReception()
        {
            try
            {
                var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();
                if (!cart.Any())
                {
                    return Json(new { success = false, message = "Cart is empty" });
                }

                foreach (var item in cart)
                {
                    var inventory = await _context.WarehouseInventories
                        .FirstOrDefaultAsync(wi => wi.WarehouseId == item.WarehouseId && wi.ProductId == item.ProductId);

                    if (inventory == null)
                    {
                        inventory = new WarehouseInventory
                        {
                            WarehouseId = item.WarehouseId,
                            ProductId = item.ProductId,
                            Quantity = item.Quantity
                        };
                        _context.WarehouseInventories.Add(inventory);
                    }
                    else
                    {
                        inventory.Quantity += item.Quantity;
                        _context.WarehouseInventories.Update(inventory);
                    }

                    var warehouseEvent = new WarehouseEvent
                    {
                        WarehouseId = item.WarehouseId,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        EventType = "Receive",
                        UserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                    };

                    _context.WarehouseEvents.Add(warehouseEvent);
                }

                await _context.SaveChangesAsync();
                HttpContext.Session.Remove("Cart");

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                // Log the exception details here
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> FinalizeIssue(int subcontractorId)
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();
            if (!cart.Any())
            {
                return Json(new { success = false, message = "Cart is empty." });
            }

            var subcontractor = await _context.Subcontractors.FindAsync(subcontractorId);
            if (subcontractor == null)
            {
                return Json(new { success = false, message = "Invalid subcontractor." });
            }

            foreach (var item in cart)
            {
                var inventory = await _context.WarehouseInventories
                    .FirstOrDefaultAsync(wi => wi.WarehouseId == item.WarehouseId && wi.ProductId == item.ProductId);

                if (inventory == null || inventory.Quantity < item.Quantity)
                {
                    return Json(new { success = false, message = $"Insufficient stock for product {item.ProductId}." });
                }

                inventory.Quantity -= item.Quantity;
                _context.WarehouseInventories.Update(inventory);

                var warehouseEvent = new WarehouseEvent
                {
                    WarehouseId = item.WarehouseId,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    EventType = "Issue",
                    UserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                    SubcontractorId = subcontractorId
                };

                _context.WarehouseEvents.Add(warehouseEvent);
            }

            await _context.SaveChangesAsync();
            HttpContext.Session.Remove("Cart");

            return Json(new { success = true });
        }

    }
}