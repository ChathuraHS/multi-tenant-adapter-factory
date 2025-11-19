using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductWebAPI.Dto;
using ProductWebAPI.Models;
using ProductWebAPI.Services;
using System.Net.Http;
using System.Text.Json;

namespace ProductWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class U_ProductController : ControllerBase
    {
        private readonly IProductService _service;
        private readonly IHttpClientFactory _clientFactory;

        public U_ProductController(IProductService service, IHttpClientFactory clientFactory)
        {
            _service = service;
            _clientFactory = clientFactory;
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

        [HttpPost("sync/{provider}")]
        public async Task<IActionResult> Sync(string provider)
        {
            var count = await _service.SyncFromProviderAsync(provider);
            return Ok(new { message = "Synced", provider, count });
        }

        [HttpGet("{provider}/{productId}/availability")]
        public async Task<IActionResult> GetAvailability(
            string provider,
            long productId,
            [FromQuery] DateTime date)
                {
                    var client = _clientFactory.CreateClient("AdapterFactory");

                    var url = $"/api/AdapterFactory/{provider}/products/{productId}/availability?date={date:yyyy-MM-dd}";

                    var res = await client.GetAsync(url);

                    if (!res.IsSuccessStatusCode)
                        return NotFound("No availability for selected date");

                    var json = await res.Content.ReadAsStringAsync();

                    if (string.IsNullOrWhiteSpace(json))
                        return NotFound("No availability for selected date");

                    var trimmed = json.Trim();
                    if (trimmed == "null" || trimmed == "[]" || trimmed == "{}")
                        return NotFound("No availability for selected date");

                    // Forward original JSON unchanged so caller sees the exact BokunAdapter payload
                    return Content(json, "application/json");
                }


    }

}
