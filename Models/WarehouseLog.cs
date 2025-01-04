using System;
using Microsoft.AspNetCore.Identity;

namespace WarehouseManagementSystem.Models
{
    public class WarehouseLog
    {
        public int Id { get; set; }
        public int WarehouseId { get; set; }
        public string Action { get; set; } // e.g., "Create", "Edit", "Delete"
        public string? Changes { get; set; } // JSON of changes
        public DateTime Timestamp { get; set; }
        public string UserId { get; set; } // User performing the action
        public Warehouse? Warehouse { get; set; }
        public IdentityUser? User { get; set; } // Navigation property for user

    }
}
