namespace CheckoutWebAPI.Models
{
    public class OrderItem
    {
        public int OrderItemId { get; set; }

        public int OrderId { get; set; }
        public Order Order { get; set; }

        // Product reference (static copy)
        public int ProductId { get; set; }
        public string ProductName { get; set; }     // Snapshot from Product service
        public string ProductDescription { get; set; } // Optional

        // User selections
        public DateTime SelectedDate { get; set; }
        public string SelectedTime { get; set; }   // "09:00", null

        public int Quantity { get; set; }

        // Prices
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public string Currency { get; set; }
    }

}
