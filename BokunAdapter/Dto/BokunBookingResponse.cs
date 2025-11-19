namespace BokunAdapter.Dto
{
    public class BokunBookingResponse
    {
        public long? bookingId { get; set; }
        public string? paymentUrl { get; set; }
        public string? status { get; set; }
        public string? currency { get; set; }
        public decimal? totalPrice { get; set; }
    }
}
