using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductWebAPI.Dto;
using ProductWebAPI.Models;

namespace ProductWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttributeController : ControllerBase
    {
        private readonly ProductDbContext _db;

        public AttributeController(ProductDbContext db)
        {
            _db = db;
        }

        //[HttpGet("{productId:int}")]
        //public async Task<ActionResult<IEnumerable<AttributeDto>>> GetByProduct(int productId)
        //{
        //    var attrs = await _db.Attributes
        //        .Where(a => a.ProductId == productId)
        //        .Select(a => new AttributeDto
        //        {
        //            AttributeId = a.AttributeId,
        //            Name = a.Name,
        //            Value = a.Value,
        //            ProductId = a.ProductId
        //        }).ToListAsync();

        //    return Ok(attrs);
        //}

        //[HttpPost]
        //public async Task<ActionResult> Create(AttributeDto dto)
        //{
        //    var attribute = new ProductAttribute
        //    {
        //        Name = dto.Name,
        //        Value = dto.Value,
        //        ProductId = dto.ProductId
        //    };

        //    await _db.Attributes.AddAsync(attribute);
        //    await _db.SaveChangesAsync();
        //    return Ok();
        //}

        [HttpDelete("{attrId:int}")]
        public async Task<ActionResult> Delete(int attrId)
        {
            var attribute = await _db.Attributes.FindAsync(attrId);
            if (attribute == null) return NotFound();

            _db.Attributes.Remove(attribute);
            await _db.SaveChangesAsync();
            return Ok();
        }
    }
}
