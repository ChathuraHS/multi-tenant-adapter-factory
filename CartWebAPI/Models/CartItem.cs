using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CartWebAPI.Models
{
    [Table("cart", Schema ="dbo")]
    public class CartItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CartItemId { get; set; }
        public int CartId { get; set; }
        public Cart Cart { get; set; }

        public int TourId { get; set; }
        public DateTime TourDate { get; set; }    // date/time of the tour instance
        public int Quantity { get; set; }
        public decimal PriceSnapshot { get; set; } // price at add-to-cart
        public string ExternalAdapterSource { get; set; } // optional
        public DateTime ReservedUntil { get; set; } // reservation expiry
        public ReservationStatus Status { get; set; } = ReservationStatus.Pending;
    }
}

public enum ReservationStatus { Pending, Reserved, Confirmed, Released }