namespace ProductWebAPI.Dto
{
    public class ProductRequestDto
    {
        public string ExternalId { get; set; }
        public string Provider { get; set; }
        public string ProductType { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public decimal Price { get; set; }
        public string Currency { get; set; }
        public string? Location { get; set; }

        public List<AttributeDto> Attributes { get; set; } = new();
        public List<AvailabilityDto> Availabilities { get; set; } = new();
        public List<string> Images { get; set; } = new();  // Only URLs!
    }


}
