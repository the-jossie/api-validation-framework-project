using ApiValidationFramework.Dtos;

namespace ApiValidationFramework.Services
{
    public interface IOrderService
    {
        Task<OrderResponseDto> CreateOrderAsync(CreateOrderDto order);
        Task<OrderResponseDto?> GetOrderByIdAsync(Guid id);
        Task<IEnumerable<OrderResponseDto>> GetAllOrdersAsync();
        Task<OrderResponseDto?> UpdateOrderAsync(Guid id, UpdateOrderDto order);
        Task<bool> DeleteOrderAsync(Guid id);
    }
}
