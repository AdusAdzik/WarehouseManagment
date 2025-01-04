using System;
using Microsoft.AspNetCore.Identity;

namespace WarehouseManagementSystem.Models
{
    public class ProductLog
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Action { get; set; } // e.g., "Create", "Edit", "Delete"
        public string? Changes { get; set; }
        public DateTime Timestamp { get; set; }
        public string UserId { get; set; } // User performing the action

        public Product? Product { get; set; } // Navigation property to Product

        public IdentityUser? User { get; set; } // Navigation property for user

    }
}
