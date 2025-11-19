namespace CheckoutWebAPI.Dtos
{
    public class CreateOrderRequest
    {
        public int UserId { get; set; }
        public string Currency { get; set; }
        public List<OrderItemDto> Items { get; set; }
    }
}
