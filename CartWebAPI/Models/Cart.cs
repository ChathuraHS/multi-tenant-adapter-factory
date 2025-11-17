namespace CartWebAPI.Models
{
    public class Cart
    {
        public int CartId { get; set; }

        // Identify user (string works for JWT, GUID, email, etc.)
        public int UserId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Relationship
        public List<CartItem> Items { get; set; } = new List<CartItem>();
    }
}
