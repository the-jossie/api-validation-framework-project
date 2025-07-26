using ApiValidationFramework.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace ApiValidationFramework.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        [HttpPost("/order")]
        public IActionResult CreateOrder([FromBody] CreateOrderDto order)
        {
            if (order == null)
                return BadRequest("Order cannot be null.");

            return Ok(new { message = "Order created", order });
        }
    }
}
