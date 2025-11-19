namespace CartWebAPI.Dtos
{
    public class ProductAvailabilityForCartDto
    {
        public DateTime Date { get; set; }
        public int AvailableUnits { get; set; }
        public decimal Price { get; set; }

    }

}
