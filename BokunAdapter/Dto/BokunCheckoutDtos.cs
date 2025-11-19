namespace BokunAdapter.Dto
{
    public class CheckoutOptionsResponse
    {
        public string CheckoutOptionType { get; set; }
        public string Label { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public List<string> PaymentMethods { get; set; }
        public List<CheckoutQuestion>? Questions { get; set; }
    }

    public class CheckoutQuestion
    {
        public string Id { get; set; }
        public string Question { get; set; }
    }

    public class CheckoutRequestDto
    {
        public string CheckoutOption { get; set; }
        public string PaymentMethod { get; set; }
        public string Source { get; set; } = "DIRECT_REQUEST"; // or SHOPPING_CART
        public DirectBooking DirectBooking { get; set; }
    }

    public class DirectBooking
    {
        public long ActivityId { get; set; }
        public string StartTimeId { get; set; }
        public int Quantity { get; set; }
    }

}
