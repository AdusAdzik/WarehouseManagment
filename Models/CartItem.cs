namespace WarehouseManagementSystem.Models
{
    public class CartItem
    {
        public int WarehouseId { get; set; }
        public int ProductId { get; set; }
        public double Quantity { get; set; }

        // Additional fields for display
        public string? Number { get; set; }
        public string? Name { get; set; }
    }

}
