using CheckoutWebAPI.Dtos;
using CheckoutWebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CheckoutWebAPI.Services
{
    public class CheckoutService : ICheckoutService
    {
        private readonly CheckoutDbContext _db;

        public CheckoutService(CheckoutDbContext db)
        {
            _db = db;
        }

        public async Task<OrderResponse> CreateOrderAsync(CreateOrderRequest request)
        {
            var order = new Order
            {
                UserId = request.UserId,
                Status = "Pending",
                Currency = request.Currency,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Items = request.Items.Select(i => new OrderItem
                {
                    ProductId = i.ProductId,
                    ProductName = i.ProductName,
                    ProductDescription = i.ProductDescription,
                    SelectedDate = i.SelectedDate,
                    SelectedTime = i.SelectedTime,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice,
                    TotalPrice = i.TotalPrice,
                    Currency = i.Currency
                }).ToList()
            };

            order.TotalAmount = order.Items.Sum(x => x.TotalPrice);

            _db.Orders.Add(order);
            await _db.SaveChangesAsync();

            return await MapToResponse(order.OrderId);
        }

        public async Task<OrderResponse> GetOrderAsync(int orderId)
        {
            return await MapToResponse(orderId);
        }

        public async Task<IEnumerable<OrderResponse>> GetAllOrdersAsync()
        {
            var orders = await _db.Orders
                .Include(o => o.Items)
                .ToListAsync();

            return orders.Select(o => new OrderResponse
            {
                OrderId = o.OrderId,
                UserId = o.UserId,
                Status = o.Status,
                TotalAmount = o.TotalAmount,
                Currency = o.Currency,
                CreatedAt = o.CreatedAt,
                Items = o.Items.Select(i => new OrderItemDto
                {
                    ProductId = i.ProductId,
                    ProductName = i.ProductName,
                    ProductDescription = i.ProductDescription,
                    SelectedDate = i.SelectedDate,
                    SelectedTime = i.SelectedTime,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice,
                    TotalPrice = i.TotalPrice,
                    Currency = i.Currency
                }).ToList()
            });
        }

        public async Task<OrderResponse> UpdateOrderStatusAsync(int orderId, UpdateOrderRequest request)
        {
            var order = await _db.Orders.FindAsync(orderId);
            if (order == null) return null;

            order.Status = request.Status;
            order.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();

            return await MapToResponse(orderId);
        }

        public async Task<bool> DeleteOrderAsync(int orderId)
        {
            var order = await _db.Orders
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);

            if (order == null) return false;

            _db.OrderItems.RemoveRange(order.Items);
            _db.Orders.Remove(order);
            await _db.SaveChangesAsync();

            return true;
        }

        private async Task<OrderResponse> MapToResponse(int orderId)
        {
            var order = await _db.Orders
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);

            if (order == null) return null;

            return new OrderResponse
            {
                OrderId = order.OrderId,
                UserId = order.UserId,
                Status = order.Status,
                TotalAmount = order.TotalAmount,
                Currency = order.Currency,
                CreatedAt = order.CreatedAt,
                Items = order.Items.Select(i => new OrderItemDto
                {
                    ProductId = i.ProductId,
                    ProductName = i.ProductName,
                    ProductDescription = i.ProductDescription,
                    SelectedDate = i.SelectedDate,
                    SelectedTime = i.SelectedTime,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice,
                    TotalPrice = i.TotalPrice,
                    Currency = i.Currency
                }).ToList()
            };
        }
    }
}
