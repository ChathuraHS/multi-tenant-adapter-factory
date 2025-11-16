using Microsoft.AspNetCore.Mvc;

namespace ProductWebAPI.Dto
{
    public class ImageUploadDto
    {
        public int ProductId { get; set; }
        public IFormFile ImageFile { get; set; }
    }
}
