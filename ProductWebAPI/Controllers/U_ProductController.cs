using BokunAdapter.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductWebAPI.Dto;
using ProductWebAPI.Models;
using ProductWebAPI.Services;
using System.Net.Http;

namespace ProductWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class U_ProductController : ControllerBase
    {
        private readonly IProductService _service;

        public U_ProductController(IProductService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            Ok(await _service.GetAllAsync());

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _service.GetByIdAsync(id);
            return result == null ? NotFound() : Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductRequestDto dto)
        {
            var id = await _service.CreateAsync(dto);
            return Ok(new { message = "Created", productId = id });
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, ProductRequestDto dto)
        {
            var ok = await _service.UpdateAsync(id, dto);
            return ok ? Ok(new { message = "Updated" }) : NotFound();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _service.DeleteAsync(id);
            return ok ? Ok(new { message = "Deleted" }) : NotFound();
        }

        //[HttpPost("sync/bokun")]
        //public async Task<IActionResult> Sync()
        //{
        //    var count = await _service.SyncFromBokunAsync();  
        //    return Ok(new { message = "Synced", count });
        //}

        [HttpPost("sync/{provider}")]
        public async Task<IActionResult> Sync(string provider)
        {
            var count = await _service.SyncFromProviderAsync(provider);
            return Ok(new { message = "Synced", provider, count });
        }

    }

}
