using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductWebAPI.Dto;
using ProductWebAPI.Models;

namespace ProductWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ProductDbContext _db;

        public ProductController(ProductDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductResponseDto>>> GetProducts()
        {
            var products = await _db.Products
                .Include(p => p.Category)
                .Include(p => p.TourImages)
                .Include(p => p.Attributes)
                .Include(p => p.TourAvailabilities)
                .Select(p => new ProductResponseDto
                {
                    ProductId = p.ProductId,
                    ExternalId = p.ExternalId,
                    Provider = p.Provider,
                    ProductType = p.ProductType,
                    Name = p.Name,
                    Description = p.Description,
                    CategoryName = p.Category.CategoryName,
                    Price = p.Price,
                    Currency = p.Currency,
                    Location = p.Location,
                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt,

                    ImageUrls = p.TourImages
                        .Select(i => i.ImageUrl)
                        .ToList(),

                    Attributes = p.Attributes
                        .Select(a => new AttributeItem
                        {
                            Name = a.Name,
                            Value = a.Value
                        })
                        .ToList(),

                    Availabilities = p.TourAvailabilities
                        .Select(a => new AvailabilityItem
                        {
                            Date = a.Date,
                            AvailableUnits = a.AvailableUnits,
                            Price = a.Price
                        })
                        .ToList()
                })
                .ToListAsync();

            return Ok(products);
        }


        [HttpPost]
        public async Task<ActionResult> CreateProduct([FromBody] ProductCreateDto dto)
        {
            var product = new Product
            {
                ExternalId = dto.ExternalId,
                Provider = dto.Provider,
                ProductType = dto.ProductType,
                Name = dto.Name,
                Description = dto.Description,
                CategoryId = dto.CategoryId,
                Price = dto.Price,
                Currency = dto.Currency,
                Location = dto.Location,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _db.Products.AddAsync(product);
            await _db.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var product = await _db.Products.FindAsync(id);
            if (product == null) return NotFound();

            _db.Products.Remove(product);
            await _db.SaveChangesAsync();
            return Ok();
        }
    }
}
