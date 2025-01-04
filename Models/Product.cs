using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WarehouseManagementSystem.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Number is required")]
        [StringLength(50, ErrorMessage = "Number cannot exceed 50 characters")]
        public string Number { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Unit is required")]
        [StringLength(10, ErrorMessage = "Unit cannot exceed 10 characters")]
        public string Unit { get; set; }

        [StringLength(50, ErrorMessage = "Description cannot exceed 50 characters")]
        public string? Description { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Cubic volume must be a positive number")]
        public double CubicVolume { get; set; }

        public string? CreatedBy { get; set; } // User ID of the creator
        public IdentityUser? Creator { get; set; } // Navigation property for the creator

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Creation timestamp
        public DateTime? DeletedAt { get; set; } // Soft delete timestamp

        public ICollection<ProductLog>? ProductLogs { get; set; }

        public ICollection<WarehouseInventory>? WarehouseInventories { get; set; } // Inventory relationship
        public ICollection<WarehouseEvent>? WarehouseEvents { get; set; } // Event relationship
    }
}
