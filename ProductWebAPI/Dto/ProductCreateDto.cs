namespace ProductWebAPI.Dto
{
    public class ProductCreateDto
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
    }
}
