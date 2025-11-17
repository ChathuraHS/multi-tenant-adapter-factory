using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CartWebAPI.Models
{
    [Table("cart", Schema ="dbo")]
    public class CartItem
    {
        public int CartItemId { get; set; }

        public int CartId { get; set; }
        public Cart Cart { get; set; }

        // Product reference from Product Microservice
        public int ProductId { get; set; }

        // From user selection
        public DateTime SelectedDate { get; set; }
        public string SelectedTime { get; set; }  // "09:00" or null if not applicable
        public int Quantity { get; set; }

        // Price snapshot at time of adding to cart
        public decimal UnitPrice { get; set; }
        public string Currency { get; set; }

        public decimal TotalPrice => UnitPrice * Quantity;

        public DateTime AddedAt { get; set; } = DateTime.UtcNow;
    }

}

public enum ReservationStatus { Pending, Reserved, Confirmed, Released }