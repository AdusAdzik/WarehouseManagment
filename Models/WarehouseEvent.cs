using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace WarehouseManagementSystem.Models
{
    public class WarehouseEvent
    {
        public int Id { get; set; }

        [Required]
        public int WarehouseId { get; set; }
        public Warehouse Warehouse { get; set; } // Navigation property to Warehouse

        [Required]
        public int ProductId { get; set; }
        public Product Product { get; set; } // Navigation property to Product

        public double Quantity { get; set; } // Quantity involved in the event

        [Required]
        public string EventType { get; set; } // e.g., "Receive", "Issue"

        public DateTime EventDate { get; set; } = DateTime.UtcNow;

        public string? UserId { get; set; }
        public IdentityUser User { get; set; } // Navigation property to User

        // Optional link to WarehouseInventory
        public int? WarehouseInventoryId { get; set; }
        public WarehouseInventory? WarehouseInventory { get; set; }

        // Subcontractor Relationship
        public int? SubcontractorId { get; set; }
        public Subcontractor? Subcontractor { get; set; }
    }
}
