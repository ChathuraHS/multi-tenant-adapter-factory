using CheckoutWebAPI.Dtos;

namespace CheckoutWebAPI.Services
{
    public interface ICheckoutService
    {
        Task<OrderResponse> CreateOrderAsync(CreateOrderRequest request);
        Task<OrderResponse> GetOrderAsync(int orderId);
        Task<IEnumerable<OrderResponse>> GetAllOrdersAsync();
        Task<OrderResponse> UpdateOrderStatusAsync(int orderId, UpdateOrderRequest request);
        Task<bool> DeleteOrderAsync(int orderId);
    }
}
