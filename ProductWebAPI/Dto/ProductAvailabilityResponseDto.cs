namespace ProductWebAPI.Dto
{
    public class ProductAvailabilityResponseDto
    {
        public int AvailabilityId { get; set; }
        public int ProductId { get; set; }
        public DateTime Date { get; set; }
        public int AvailableUnits { get; set; }
        public decimal Price { get; set; }
    }
}
