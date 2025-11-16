using BokunAdapter.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BokunAdapter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BokunController : ControllerBase
    {
        private readonly BokunService _bokun;

        public BokunController(BokunService bokun)
        {
            _bokun = bokun;
        }

        [HttpGet("products")]
        public async Task<IActionResult> GetProducts()
        {
            var result = await _bokun.FetchProductsAsync();
            return Ok(result);
        }
    }

}
