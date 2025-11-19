using BokunAdapter.Dto;
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

        [HttpGet("products/{id}/availability")]
        public async Task<IActionResult> GetAvailability(long id, [FromQuery] DateTime date)
        {
            if (date == default)
                return BadRequest("Date is required");

            var availability = await _bokun.GetAvailabilityAsync(id, date);

            if (availability == null)
                return Ok(new List<ProductAvailabilityDto>()); // return empty list instead of 404


            return Ok(availability);
        }

        // GET checkout options for a specific activity
        [HttpGet("checkout/options/{activityId}")]
        public async Task<IActionResult> GetCheckoutOptions(long activityId)
        {
            try
            {
                var options = await _bokun.GetCheckoutOptionsAsync(activityId);
                return Ok(options);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Failed to fetch checkout options", details = ex.Message });
            }
        }


        // POST submit a checkout request
        [HttpPost("checkout/submit")]
        public async Task<IActionResult> SubmitCheckout([FromBody] CheckoutRequestDto request)
        {
            if (request == null)
                return BadRequest("Checkout request body is required");

            try
            {
                var response = await _bokun.SubmitCheckoutAsync(request);
                return Ok(response); // raw JSON with booking info, payment URLs, etc.
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Checkout failed", details = ex.Message });
            }
        }


    }

}
