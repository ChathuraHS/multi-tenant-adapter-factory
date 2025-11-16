namespace CartWebAPI.Models
{
    public class Cart
    {
        public int CartId { get; set; }
        public string UserId { get; set; }         // or int, depending on auth
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<CartItem> Items { get; set; } = new List<CartItem>();
    }
}
