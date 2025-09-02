using ApiValidationFramework.Dtos;
using ApiValidationFramework.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiValidationFramework.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost("/order")]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto order)
        {
            if (order == null)
                return BadRequest("Order cannot be null.");

            if (order.EndDate < order.StartDate)
                return BadRequest("EndDate must be after StartDate");

            var createdOrder = await _orderService.CreateOrderAsync(order);
            return Ok(new { message = "Order created", order = createdOrder });
        }

        [HttpGet("/orders")]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return Ok(orders);
        }

        [HttpGet("/order/{id}")]
        public async Task<IActionResult> GetOrder(Guid id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null)
                return NotFound($"Order with ID {id} not found.");

            return Ok(order);
        }

        [HttpPut("/order/{id}")]
        public async Task<IActionResult> UpdateOrder(Guid id, [FromBody] UpdateOrderDto order)
        {
            if (order == null)
                return BadRequest("Order cannot be null.");

            if (order.EndDate < order.StartDate)
                return BadRequest("EndDate must be after StartDate");

            var updatedOrder = await _orderService.UpdateOrderAsync(id, order);
            if (updatedOrder == null)
                return NotFound($"Order with ID {id} not found.");

            return Ok(new { message = "Order updated", order = updatedOrder });
        }

        [HttpDelete("/order/{id}")]
        public async Task<IActionResult> DeleteOrder(Guid id)
        {
            var deleted = await _orderService.DeleteOrderAsync(id);
            if (!deleted)
                return NotFound($"Order with ID {id} not found.");

            return Ok(new { message = "Order deleted successfully" });
        }
    }
}
