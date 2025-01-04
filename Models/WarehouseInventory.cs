using System.ComponentModel.DataAnnotations;

namespace WarehouseManagementSystem.Models
{
    public class WarehouseInventory
    {
        public int Id { get; set; }

        [Required]
        public int WarehouseId { get; set; }
        public Warehouse Warehouse { get; set; } // Navigation property to Warehouse

        [Required]
        public int ProductId { get; set; }
        public Product Product { get; set; } // Navigation property to Product

        [Range(0, double.MaxValue, ErrorMessage = "Quantity must be a positive number.")]
        public double Quantity { get; set; } // Quantity in the warehouse
    }
}
