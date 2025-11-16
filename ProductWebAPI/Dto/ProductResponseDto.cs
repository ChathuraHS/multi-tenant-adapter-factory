namespace ProductWebAPI.Dto
{
    public class ProductResponseDto
    {
        public int ProductId { get; set; }
        public string ExternalId { get; set; }
        public string Provider { get; set; }
        public string ProductType { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string CategoryName { get; set; }
        public decimal Price { get; set; }
        public string Currency { get; set; }
        public string? Location { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public List<string> ImageUrls { get; set; }
        public List<AttributeItem> Attributes { get; set; }
        public List<AvailabilityItem> Availabilities { get; set; }
    }
}
