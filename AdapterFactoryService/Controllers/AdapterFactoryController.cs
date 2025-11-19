using AdapterFactoryService.Adapters.Factories;
using AdapterFactoryService.Dtos;
using BokunAdapter.Dto;
using Microsoft.AspNetCore.Mvc;

namespace AdapterFactoryService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdapterFactoryController : ControllerBase
    {
        //The controller depends on IProductAdapterFactory. Stored as _factory so the methods of the factory can be used.
        private readonly IProductAdapterFactory _factory;

        public AdapterFactoryController(IProductAdapterFactory factory)
        {
            _factory = factory;
        }

        //URL Format : GET /api/Adapter/product/{externalId}?provider=bokun
        [HttpGet("product/{externalId}")]
        public async Task<ActionResult<ProductResponseDto>> GetProduct(
            string externalId,
            [FromQuery] string provider)
        {
            //The factory returns the correct adapter
            var adapter = _factory.GetAdapter(provider);

            //The controller calls that adapter:
            var result = await adapter.GetProductAsync(externalId);
            return Ok(result);
        }

        //URL Format : GET /api/Adapter/bokun/products
        [HttpGet("{provider}/products")]
        public async Task<ActionResult<List<ProductSyncDto>>> GetProducts(string provider)
        {
            //The factory returns the correct adapter
            var adapter = _factory.GetAdapter(provider);

            //The controller calls that adapter:
            var products = await adapter.GetAllProductsAsync(); // This method must be in each provider adapsters.
            return Ok(products);
        }

        // URL Format : GET /api/Adapter/{provider}/products/{productId}/availability?date=2024-07-01
        // Updated to forward raw JSON from provider so callers get exactly what BokunAdapter returns.
        [HttpGet("{provider}/products/{productId}/availability")]
        public async Task<IActionResult> GetAvailability(
            string provider,
            long productId,
            [FromQuery] DateTime date)
        {
            var adapter = _factory.GetAdapter(provider);

            // Use the raw JSON passthrough to preserve response shape from the provider
            var json = await adapter.GetAvailabilityRawAsync(productId, date);

            if (json == null)
                return NotFound("No availability found for the selected date");

            // Return the original JSON with correct content-type so callers receive identical payload
            return Content(json, "application/json");
        }


    }
}
