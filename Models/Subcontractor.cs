using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace WarehouseManagementSystem.Models
{
    public class Subcontractor
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; }

        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string? Email { get; set; }

        [Phone(ErrorMessage = "Invalid Phone Number")]
        public string? Phone { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string? Description { get; set; }

        // Soft delete support
        public DateTime? DeletedAt { get; set; }

        public string? UserId { get; set; } // User who manages the subcontractor
        public IdentityUser? User { get; set; } // Navigation property

        public ICollection<SubcontractorLog>? SubcontractorLogs { get; set; } // Logs for CRUD operations


        // Relationship to WarehouseEvents
        public ICollection<WarehouseEvent>? WarehouseEvents { get; set; }
    }
}
