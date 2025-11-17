namespace AdapterFactoryService.Dtos
{
    public class ProductResponseDto
    {
        public int ProductId { get; set; }
        public string ExternalId { get; set; }
        public string Provider { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
    }
}
