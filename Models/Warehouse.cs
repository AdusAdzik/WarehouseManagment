using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace WarehouseManagementSystem.Models
{
    public class Warehouse
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; }

        [StringLength(200, ErrorMessage = "Address cannot exceed 200 characters")]
        public string Address { get; set; }

        [Required(ErrorMessage = "City is required")]
        public string City { get; set; }

        [Required(ErrorMessage = "Country is required")]
        public string Country { get; set; }

        [StringLength(10, ErrorMessage = "Postal code cannot exceed 10 characters")]
        public string? PostalCode { get; set; }

        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string? Email { get; set; }

        [Phone(ErrorMessage = "Invalid Phone Number")]
        public string? Phone { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Capacity must be a positive number")]
        public int Capacity { get; set; }

        public bool IsActive { get; set; }

        // New property to associate warehouses with users - every user has their own warehoues that they manage
        public string? UserId { get; set; }
        public IdentityUser? User { get; set; }

        // Navigation property for logs
        public ICollection<WarehouseLog>? WarehouseLogs { get; set; }

        public ICollection<WarehouseInventory>? WarehouseInventories { get; set; } // Inventory relationship
        public ICollection<WarehouseEvent>? WarehouseEvents { get; set; } // Event relationship
    }
}
