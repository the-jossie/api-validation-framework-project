using Xunit;
using ApiValidationFramework.Controllers;
using ApiValidationFramework.Dtos;
using Microsoft.AspNetCore.Mvc;

public class OrderControllerUnitTests
{
    [Fact]
    public void CreateOrder_ValidOrder_ReturnsOk()
    {
        var controller = new OrderController();
        var order = new CreateOrderDto
        {
            ProductName = "Nike AF1",
            Quantity = 10,
            StartDate = DateTime.Parse("2025-08-01"),
            EndDate = DateTime.Parse("2025-08-10")
        };

        var result = controller.CreateOrder(order) as OkObjectResult;

        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
        var message = result.Value?.ToString();
        Assert.Contains("Order created", message);
    }

    [Fact]
    public void CreateOrder_EndDateBeforeStartDate_ReturnsBadRequest()
    {
        var controller = new OrderController();
        var order = new CreateOrderDto
        {
            ProductName = "Nike AF1",
            Quantity = 10,
            StartDate = DateTime.Parse("2025-08-10"),
            EndDate = DateTime.Parse("2025-08-01")
        };

        var result = controller.CreateOrder(order) as BadRequestObjectResult;

        Assert.NotNull(result);
        Assert.Equal(400, result.StatusCode);
        Assert.Equal("EndDate must be after StartDate", result.Value);
    }
}
