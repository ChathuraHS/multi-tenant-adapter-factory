namespace CheckoutWebAPI.Models
{
    public class Order
    {
        public int OrderId { get; set; }

        // Identity
        public int UserId { get; set; }

        // Status
        public string Status { get; set; } = "Pending";
        // Pending → Paid → Completed → Cancelled

        // Financial
        public decimal TotalAmount { get; set; }
        public string Currency { get; set; }

        // Timestamps
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Relationships
        public List<OrderItem> Items { get; set; } = new();
    }
}
