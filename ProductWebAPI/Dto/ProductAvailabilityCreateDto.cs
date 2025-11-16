namespace ProductWebAPI.Dto
{
    public class ProductAvailabilityCreateDto
    {
        public int ProductId { get; set; }
        public DateTime Date { get; set; }
        public int AvailableUnits { get; set; }
        public decimal Price { get; set; }
    }
}
