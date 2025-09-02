using ApiValidationFramework.Dtos;

namespace ApiValidationFramework.Services
{
    public class OrderService : IOrderService
    {
        private readonly Dictionary<Guid, OrderResponseDto> _orders = new();
        private readonly object _lock = new();

        public Task<OrderResponseDto> CreateOrderAsync(CreateOrderDto order)
        {
            var orderResponse = new OrderResponseDto
            {
                Id = Guid.NewGuid(),
                ProductName = order.ProductName,
                Quantity = order.Quantity,
                StartDate = order.StartDate,
                EndDate = order.EndDate,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = null
            };

            lock (_lock)
            {
                _orders[orderResponse.Id] = orderResponse;
            }

            return Task.FromResult(orderResponse);
        }

        public Task<OrderResponseDto?> GetOrderByIdAsync(Guid id)
        {
            lock (_lock)
            {
                _orders.TryGetValue(id, out var order);
                return Task.FromResult(order);
            }
        }

        public Task<IEnumerable<OrderResponseDto>> GetAllOrdersAsync()
        {
            lock (_lock)
            {
                return Task.FromResult(_orders.Values.AsEnumerable());
            }
        }

        public Task<OrderResponseDto?> UpdateOrderAsync(Guid id, UpdateOrderDto order)
        {
            lock (_lock)
            {
                if (!_orders.TryGetValue(id, out var existingOrder))
                {
                    return Task.FromResult<OrderResponseDto?>(null);
                }

                var updatedOrder = new OrderResponseDto
                {
                    Id = existingOrder.Id,
                    ProductName = order.ProductName,
                    Quantity = order.Quantity,
                    StartDate = order.StartDate,
                    EndDate = order.EndDate,
                    CreatedAt = existingOrder.CreatedAt,
                    UpdatedAt = DateTime.UtcNow
                };

                _orders[id] = updatedOrder;
                return Task.FromResult<OrderResponseDto?>(updatedOrder);
            }
        }

        public Task<bool> DeleteOrderAsync(Guid id)
        {
            lock (_lock)
            {
                return Task.FromResult(_orders.Remove(id));
            }
        }
    }
}
