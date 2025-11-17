using AdapterFactoryService.Adapters.Factories;
using AdapterFactoryService.Dtos;
using BokunAdapter.Dto;
using Microsoft.AspNetCore.Mvc;

namespace AdapterFactoryService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdapterController : ControllerBase
    {
        private readonly IProductAdapterFactory _factory;

        public AdapterController(IProductAdapterFactory factory)
        {
            _factory = factory;
        }

        [HttpGet("product/{externalId}")]
        public async Task<ActionResult<ProductResponseDto>> GetProduct(
            string externalId,
            [FromQuery] string provider)
        {
            var adapter = _factory.GetAdapter(provider);
            var result = await adapter.GetProductAsync(externalId);
            return Ok(result);
        }

        [HttpGet("{provider}/products")]
        public async Task<ActionResult<List<ProductSyncDto>>> GetProducts(string provider)
        {
            var adapter = _factory.GetAdapter(provider);
            var products = await adapter.GetAllProductsAsync(); // Add this method in each provider adapter
            return Ok(products);
        }


    }
}
