using CartWebAPI.Dtos;

namespace CartWebAPI.Service
{
    public interface ICartService
    {
        Task<CartResponseDto> GetCartByUserIdAsync(int userId);
        Task<CartResponseDto> AddItemAsync(int userId, CartItemRequestDto dto);
        Task<CartResponseDto> RemoveItemAsync(int userId, int cartItemId);

    }

}
