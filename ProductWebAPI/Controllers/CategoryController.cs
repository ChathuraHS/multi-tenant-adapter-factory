using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductWebAPI.Dto;
using ProductWebAPI.Models;

namespace ProductWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ProductDbContext _db;

        public CategoryController(ProductDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategories()
        {
            var categories = await _db.Categories
                .Select(c => new CategoryDto
                {
                    CategoryId = c.CategoryId,
                    CategoryName = c.CategoryName
                }).ToListAsync();

            return Ok(categories);
        }

        [HttpPost]
        public async Task<ActionResult> Create(CategoryDto dto)
        {
            var category = new Category
            {
                CategoryName = dto.CategoryName
            };

            await _db.Categories.AddAsync(category);
            await _db.SaveChangesAsync();
            return Ok();
        }
    }
}
