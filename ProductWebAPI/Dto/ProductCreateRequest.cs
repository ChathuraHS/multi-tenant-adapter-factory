namespace ProductWebAPI.Dto
{
    using Microsoft.AspNetCore.Http;

    public class ProductCreateRequest
    {
        // Product base info
        public string ExternalId { get; set; }
        public string Provider { get; set; }
        public string ProductType { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public decimal Price { get; set; }
        public string Currency { get; set; }
        public string Location { get; set; }

        // Images
        public List<IFormFile> Images { get; set; }

        // Attributes
        public List<AttributeItem> Attributes { get; set; }

        // Availability
        public List<AvailabilityItem> Availabilities { get; set; }
    }

    public class AttributeItem
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }

    public class AvailabilityItem
    {
        public DateTime Date { get; set; }
        public int AvailableUnits { get; set; }
        public decimal Price { get; set; }
    }

}
