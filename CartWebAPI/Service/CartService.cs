using CartWebAPI.Dtos;
using CartWebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CartWebAPI.Service
{
    public class CartService : ICartService
    {
        private readonly CartDbContext _db;

        public CartService(CartDbContext db)
        {
            _db = db;
        }

        // ---------------------------------------------------
        // GET CART BY USER ID
        // ---------------------------------------------------
        public async Task<CartResponseDto> GetCartByUserIdAsync(int userId)
        {
            var cart = await _db.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = userId
                };

                _db.Carts.Add(cart);
                await _db.SaveChangesAsync();
            }

            return MapToCartDto(cart);
        }

        // ---------------------------------------------------
        // ADD ITEM TO CART
        // ---------------------------------------------------
        public async Task<CartResponseDto> AddItemAsync(int userId, CartItemRequestDto dto)
        {
            var cart = await _db.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = userId
                };

                _db.Carts.Add(cart);
            }

            var item = new CartItem
            {
                ProductId = dto.ProductId,
                SelectedDate = dto.SelectedDate,
                Quantity = dto.Quantity,
                UnitPrice = dto.UnitPrice,
                Currency = dto.Currency,
                CartId = cart.CartId
            };

            cart.Items.Add(item);

            await _db.SaveChangesAsync();

            return MapToCartDto(cart);
        }

        // ---------------------------------------------------
        // REMOVE ITEM FROM CART
        // ---------------------------------------------------
        public async Task<CartResponseDto> RemoveItemAsync(int userId, int cartItemId)
        {
            var cart = await _db.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
                return null;

            var item = cart.Items.FirstOrDefault(i => i.CartItemId == cartItemId);

            if (item != null)
            {
                cart.Items.Remove(item);
                _db.CartItems.Remove(item);
                await _db.SaveChangesAsync();
            }

            return MapToCartDto(cart);
        }

        // ---------------------------------------------------
        // DTO MAPPER
        // ---------------------------------------------------
        private CartResponseDto MapToCartDto(Cart cart)
        {
            return new CartResponseDto
            {
                CartId = cart.CartId,
                UserId = cart.UserId,
                Items = cart.Items.Select(i => new CartItemResponseDto
                {
                    CartItemId = i.CartItemId,
                    ProductId = i.ProductId,
                    SelectedDate = i.SelectedDate,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice,
                    Currency = i.Currency
                }).ToList()
            };
        }
    }
}
