using Xunit;
using ApiValidationFramework.Controllers;
using ApiValidationFramework.Dtos;
using ApiValidationFramework.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using FluentAssertions;

public class OrderControllerUnitTests
{
    private readonly Mock<IOrderService> _mockOrderService;
    private readonly OrderController _controller;

    public OrderControllerUnitTests()
    {
        _mockOrderService = new Mock<IOrderService>();
        _controller = new OrderController(_mockOrderService.Object);
    }

    #region Create Order Tests

    [Fact]
    public async Task CreateOrder_ValidOrder_ReturnsOk()
    {
        var order = new CreateOrderDto
        {
            ProductName = "Nike AF1",
            Quantity = 10,
            StartDate = DateTime.Parse("2025-08-01"),
            EndDate = DateTime.Parse("2025-08-10")
        };

        var expectedResponse = new OrderResponseDto
        {
            Id = Guid.NewGuid(),
            ProductName = order.ProductName,
            Quantity = order.Quantity,
            StartDate = order.StartDate,
            EndDate = order.EndDate,
            CreatedAt = DateTime.UtcNow
        };

        _mockOrderService.Setup(x => x.CreateOrderAsync(order))
            .ReturnsAsync(expectedResponse);

        var result = await _controller.CreateOrder(order) as OkObjectResult;

        result.Should().NotBeNull();
        result!.StatusCode.Should().Be(200);
        var message = result.Value?.ToString();
        message.Should().Contain("Order created");
    }

    [Fact]
    public async Task CreateOrder_EndDateBeforeStartDate_ReturnsBadRequest()
    {
        var order = new CreateOrderDto
        {
            ProductName = "Nike AF1",
            Quantity = 10,
            StartDate = DateTime.Parse("2025-08-10"),
            EndDate = DateTime.Parse("2025-08-01")
        };

        var result = await _controller.CreateOrder(order) as BadRequestObjectResult;

        result.Should().NotBeNull();
        result!.StatusCode.Should().Be(400);
        result.Value.Should().Be("EndDate must be after StartDate");
    }

    [Fact]
    public async Task CreateOrder_NullOrder_ReturnsBadRequest()
    {
        var result = await _controller.CreateOrder(null!) as BadRequestObjectResult;

        result.Should().NotBeNull();
        result!.StatusCode.Should().Be(400);
        result.Value.Should().Be("Order cannot be null.");
    }

    #endregion

    #region Get Order Tests

    [Fact]
    public async Task GetOrder_ExistingOrder_ReturnsOk()
    {
        var orderId = Guid.NewGuid();
        var expectedOrder = new OrderResponseDto
        {
            Id = orderId,
            ProductName = "Nike AF1",
            Quantity = 10,
            StartDate = DateTime.Parse("2025-08-01"),
            EndDate = DateTime.Parse("2025-08-10"),
            CreatedAt = DateTime.UtcNow
        };

        _mockOrderService.Setup(x => x.GetOrderByIdAsync(orderId))
            .ReturnsAsync(expectedOrder);

        var result = await _controller.GetOrder(orderId) as OkObjectResult;

        result.Should().NotBeNull();
        result!.StatusCode.Should().Be(200);
        result.Value.Should().Be(expectedOrder);
    }

    [Fact]
    public async Task GetOrder_NonExistentOrder_ReturnsNotFound()
    {
        var orderId = Guid.NewGuid();
        _mockOrderService.Setup(x => x.GetOrderByIdAsync(orderId))
            .ReturnsAsync((OrderResponseDto?)null);

        var result = await _controller.GetOrder(orderId) as NotFoundObjectResult;

        result.Should().NotBeNull();
        result!.StatusCode.Should().Be(404);
        result.Value.Should().Be($"Order with ID {orderId} not found.");
    }

    [Fact]
    public async Task GetAllOrders_ReturnsOk()
    {
        var expectedOrders = new List<OrderResponseDto>
        {
            new() { Id = Guid.NewGuid(), ProductName = "Nike AF1", Quantity = 10 },
            new() { Id = Guid.NewGuid(), ProductName = "Adidas Superstar", Quantity = 5 }
        };

        _mockOrderService.Setup(x => x.GetAllOrdersAsync())
            .ReturnsAsync(expectedOrders);

        var result = await _controller.GetAllOrders() as OkObjectResult;

        result.Should().NotBeNull();
        result!.StatusCode.Should().Be(200);
        result.Value.Should().Be(expectedOrders);
    }

    #endregion

    #region Update Order Tests

    [Fact]
    public async Task UpdateOrder_ValidUpdate_ReturnsOk()
    {
        var orderId = Guid.NewGuid();
        var updateOrder = new UpdateOrderDto
        {
            ProductName = "Adidas Superstar",
            Quantity = 5,
            StartDate = DateTime.Parse("2025-08-02"),
            EndDate = DateTime.Parse("2025-08-12")
        };

        var expectedResponse = new OrderResponseDto
        {
            Id = orderId,
            ProductName = updateOrder.ProductName,
            Quantity = updateOrder.Quantity,
            StartDate = updateOrder.StartDate,
            EndDate = updateOrder.EndDate,
            CreatedAt = DateTime.UtcNow.AddDays(-1),
            UpdatedAt = DateTime.UtcNow
        };

        _mockOrderService.Setup(x => x.UpdateOrderAsync(orderId, updateOrder))
            .ReturnsAsync(expectedResponse);

        var result = await _controller.UpdateOrder(orderId, updateOrder) as OkObjectResult;

        result.Should().NotBeNull();
        result!.StatusCode.Should().Be(200);
        var message = result.Value?.ToString();
        message.Should().Contain("Order updated");
    }

    [Fact]
    public async Task UpdateOrder_EndDateBeforeStartDate_ReturnsBadRequest()
    {
        var orderId = Guid.NewGuid();
        var updateOrder = new UpdateOrderDto
        {
            ProductName = "Adidas Superstar",
            Quantity = 5,
            StartDate = DateTime.Parse("2025-08-12"),
            EndDate = DateTime.Parse("2025-08-02")
        };

        var result = await _controller.UpdateOrder(orderId, updateOrder) as BadRequestObjectResult;

        result.Should().NotBeNull();
        result!.StatusCode.Should().Be(400);
        result.Value.Should().Be("EndDate must be after StartDate");
    }

    [Fact]
    public async Task UpdateOrder_NonExistentOrder_ReturnsNotFound()
    {
        var orderId = Guid.NewGuid();
        var updateOrder = new UpdateOrderDto
        {
            ProductName = "Adidas Superstar",
            Quantity = 5,
            StartDate = DateTime.Parse("2025-08-02"),
            EndDate = DateTime.Parse("2025-08-12")
        };

        _mockOrderService.Setup(x => x.UpdateOrderAsync(orderId, updateOrder))
            .ReturnsAsync((OrderResponseDto?)null);

        var result = await _controller.UpdateOrder(orderId, updateOrder) as NotFoundObjectResult;

        result.Should().NotBeNull();
        result!.StatusCode.Should().Be(404);
        result.Value.Should().Be($"Order with ID {orderId} not found.");
    }

    #endregion

    #region Delete Order Tests

    [Fact]
    public async Task DeleteOrder_ExistingOrder_ReturnsOk()
    {
        var orderId = Guid.NewGuid();
        _mockOrderService.Setup(x => x.DeleteOrderAsync(orderId))
            .ReturnsAsync(true);

        var result = await _controller.DeleteOrder(orderId) as OkObjectResult;

        result.Should().NotBeNull();
        result!.StatusCode.Should().Be(200);
        var message = result.Value?.ToString();
        message.Should().Contain("Order deleted successfully");
    }

    [Fact]
    public async Task DeleteOrder_NonExistentOrder_ReturnsNotFound()
    {
        var orderId = Guid.NewGuid();
        _mockOrderService.Setup(x => x.DeleteOrderAsync(orderId))
            .ReturnsAsync(false);

        var result = await _controller.DeleteOrder(orderId) as NotFoundObjectResult;

        result.Should().NotBeNull();
        result!.StatusCode.Should().Be(404);
        result.Value.Should().Be($"Order with ID {orderId} not found.");
    }

    #endregion
}
