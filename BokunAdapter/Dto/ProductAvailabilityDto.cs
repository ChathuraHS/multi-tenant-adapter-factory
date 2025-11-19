namespace BokunAdapter.Dto
{
    public class ProductAvailabilityDto
    {
        public DateTime Date { get; set; }
        public int AvailableUnits { get; set; }
        public decimal Price { get; set; }
        public decimal UnitPrice { get; set; }
        public string Currency { get; set; }
    }
}
