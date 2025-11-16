using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductWebAPI.Dto;
using ProductWebAPI.Models;

namespace ProductWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductAvailabilityController : ControllerBase
    {
        private readonly ProductDbContext _db;

        public ProductAvailabilityController(ProductDbContext db)
        {
            _db = db;
        }

        [HttpGet("product/{productId:int}")]
        public async Task<ActionResult<IEnumerable<ProductAvailabilityResponseDto>>> GetByProduct(int productId)
        {
            var availabilities = await _db.ProductAvailabilities
                .Where(p => p.ProductId == productId)
                .Select(p => new ProductAvailabilityResponseDto
                {
                    AvailabilityId = p.AvailabilityId,
                    ProductId = p.ProductId,
                    Date = p.Date,
                    AvailableUnits = p.AvailableUnits,
                    Price = p.Price
                }).ToListAsync();

            return Ok(availabilities);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] ProductAvailabilityCreateDto dto)
        {
            var avail = new ProductAvailability
            {
                ProductId = dto.ProductId,
                Date = dto.Date,
                AvailableUnits = dto.AvailableUnits,
                Price = dto.Price
            };

            await _db.ProductAvailabilities.AddAsync(avail);
            await _db.SaveChangesAsync();
            return Ok(avail);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var avail = await _db.ProductAvailabilities.FindAsync(id);
            if (avail == null) return NotFound();

            _db.ProductAvailabilities.Remove(avail);
            await _db.SaveChangesAsync();
            return Ok();
        }
    }
}
