using Microsoft.AspNetCore.Mvc;
using ProductWebAPI.Dto;
using ProductWebAPI.Models;

namespace ProductWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductImageController : ControllerBase
    {
        private readonly ProductDbContext _db;
        private readonly IWebHostEnvironment _env;

        public ProductImageController(ProductDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        [HttpPost("upload")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadImage([FromForm] ImageUploadDto request)
        {
            if (request.ImageFile == null || request.ImageFile.Length == 0)
                return BadRequest("No image uploaded");

            var uploadFolder = Path.Combine(_env.WebRootPath, "product_images");
            if (!Directory.Exists(uploadFolder))
                Directory.CreateDirectory(uploadFolder);

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(request.ImageFile.FileName);
            var filePath = Path.Combine(uploadFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await request.ImageFile.CopyToAsync(stream);
            }

            var image = new ProductImage
            {
                ProductId = request.ProductId,
                ImageUrl = $"product_images/{fileName}"
            };

            _db.ProductImages.Add(image);
            await _db.SaveChangesAsync();

            return Ok(new { message = "Uploaded successfully", file = image.ImageUrl });
        }
    }
}
