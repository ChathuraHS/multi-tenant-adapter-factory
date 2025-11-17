using CartWebAPI.Dtos;
using CartWebAPI.Service;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CartWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        // Get all cart items for a user
        [HttpGet("{userId}")]
        public async Task<ActionResult<CartResponseDto>> GetUserCart(int userId)
        {
            var cart = await _cartService.GetCartByUserIdAsync(userId);
            return Ok(cart);
        }

        // Add to cart
        [HttpPost("{userId}/items")]
        public async Task<IActionResult> AddToCart(int userId, [FromBody] CartItemRequestDto dto)
        {
            var cart = await _cartService.AddItemAsync(userId, dto);
            return Ok(cart);
        }

        // Remove item
        [HttpDelete("{userId}/items/{cartItemId}")]
        public async Task<IActionResult> RemoveFromCart(int userId, int cartItemId)
        {
            var cart = await _cartService.RemoveItemAsync(userId, cartItemId);
            return Ok(cart);
        }
    }

}
