namespace CartWebAPI.Dtos
{
    public class CartResponseDto
    {
        public int CartId { get; set; }
        public int UserId { get; set; }
        public List<CartItemResponseDto> Items { get; set; }
        public decimal CartTotal => Items.Sum(i => i.Total);
    }
}
