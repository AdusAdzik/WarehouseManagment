using System;
using Microsoft.AspNetCore.Identity;

namespace WarehouseManagementSystem.Models
{
    public class SubcontractorLog
    {
        public int Id { get; set; }
        public int SubcontractorId { get; set; }
        public string Action { get; set; } // e.g., "Create", "Edit", "Delete"
        public string? Changes { get; set; } // Details of the changes
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public string UserId { get; set; } // User who performed the action

        public Subcontractor? Subcontractor { get; set; } // Navigation property
        public IdentityUser? User { get; set; } // Navigation property
    }
}
