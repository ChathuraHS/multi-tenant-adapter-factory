namespace CartWebAPI.Dtos
{
    public class CartItemRequestDto
    {
        public int ProductId { get; set; }
        public DateTime SelectedDate { get; set; }
        public int Quantity { get; set; }

        // Availability details obtained from Bokun
        public decimal UnitPrice { get; set; }
        public string Currency { get; set; }
    }
}
