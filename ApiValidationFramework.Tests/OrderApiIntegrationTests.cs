using System.Net;
using System.Net.Http.Json;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using FluentAssertions;
using ApiValidationFramework.Tests;

public class OrderApiIntegrationTests : IClassFixture<TestWebApplicationFactory>
{
    private readonly HttpClient _client;

    public OrderApiIntegrationTests(TestWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    #region Create Order Tests

    [Fact]
    public async Task PostOrder_EndDateBeforeStartDate_ReturnsBadRequest()
    {
        var order = new {
            productName = "Nike AF1",
            quantity = 10,
            startDate = "2025-08-10",
            endDate = "2025-08-01"
        };

        var response = await _client.PostAsJsonAsync("/order", order);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var content = await response.Content.ReadAsStringAsync();
        content.Should().Contain("EndDate must be after StartDate");
    }

    [Fact]
    public async Task PostOrder_ValidOrder_ReturnsOk()
    {
        var order = new {
            productName = "Nike AF1",
            quantity = 10,
            startDate = "2025-08-01",
            endDate = "2025-08-10"
        };

        var response = await _client.PostAsJsonAsync("/order", order);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadAsStringAsync();
        result.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task PostOrder_NullOrder_ReturnsBadRequest()
    {
        var response = await _client.PostAsJsonAsync("/order", (object)null!);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    #endregion

    #region Get Order Tests

    [Fact]
    public async Task GetOrder_ExistingOrder_ReturnsOk()
    {
        // First create an order
        var createOrder = new {
            productName = "Nike AF1",
            quantity = 10,
            startDate = "2025-08-01",
            endDate = "2025-08-10"
        };

        var createResponse = await _client.PostAsJsonAsync("/order", createOrder);
        createResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var createdOrderJson = await createResponse.Content.ReadAsStringAsync();
        var createdOrder = System.Text.Json.JsonSerializer.Deserialize<System.Text.Json.JsonElement>(createdOrderJson);
        var orderId = createdOrder.GetProperty("order").GetProperty("id").GetString();

        // Then get the order
        var getResponse = await _client.GetAsync($"/order/{orderId}");
        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetOrder_NonExistentOrder_ReturnsNotFound()
    {
        var nonExistentId = Guid.NewGuid();
        var response = await _client.GetAsync($"/order/{nonExistentId}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetAllOrders_ReturnsOk()
    {
        var response = await _client.GetAsync("/orders");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    #endregion

    #region Update Order Tests

    [Fact]
    public async Task PutOrder_ValidUpdate_ReturnsOk()
    {
        // First create an order
        var createOrder = new {
            productName = "Nike AF1",
            quantity = 10,
            startDate = "2025-08-01",
            endDate = "2025-08-10"
        };

        var createResponse = await _client.PostAsJsonAsync("/order", createOrder);
        createResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var createdOrderJson = await createResponse.Content.ReadAsStringAsync();
        var createdOrder = System.Text.Json.JsonSerializer.Deserialize<System.Text.Json.JsonElement>(createdOrderJson);
        var orderId = createdOrder.GetProperty("order").GetProperty("id").GetString();

        // Then update the order
        var updateOrder = new {
            productName = "Adidas Superstar",
            quantity = 5,
            startDate = "2025-08-02",
            endDate = "2025-08-12"
        };

        var updateResponse = await _client.PutAsJsonAsync($"/order/{orderId}", updateOrder);
        updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task PutOrder_EndDateBeforeStartDate_ReturnsBadRequest()
    {
        // First create an order
        var createOrder = new {
            productName = "Nike AF1",
            quantity = 10,
            startDate = "2025-08-01",
            endDate = "2025-08-10"
        };

        var createResponse = await _client.PostAsJsonAsync("/order", createOrder);
        createResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var createdOrderJson = await createResponse.Content.ReadAsStringAsync();
        var createdOrder = System.Text.Json.JsonSerializer.Deserialize<System.Text.Json.JsonElement>(createdOrderJson);
        var orderId = createdOrder.GetProperty("order").GetProperty("id").GetString();

        // Then try to update with invalid dates
        var updateOrder = new {
            productName = "Adidas Superstar",
            quantity = 5,
            startDate = "2025-08-12",
            endDate = "2025-08-02"
        };

        var updateResponse = await _client.PutAsJsonAsync($"/order/{orderId}", updateOrder);
        updateResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task PutOrder_NonExistentOrder_ReturnsNotFound()
    {
        var nonExistentId = Guid.NewGuid();
        var updateOrder = new {
            productName = "Adidas Superstar",
            quantity = 5,
            startDate = "2025-08-02",
            endDate = "2025-08-12"
        };

        var response = await _client.PutAsJsonAsync($"/order/{nonExistentId}", updateOrder);
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    #endregion

    #region Delete Order Tests

    [Fact]
    public async Task DeleteOrder_ExistingOrder_ReturnsOk()
    {
        // First create an order
        var createOrder = new {
            productName = "Nike AF1",
            quantity = 10,
            startDate = "2025-08-01",
            endDate = "2025-08-10"
        };

        var createResponse = await _client.PostAsJsonAsync("/order", createOrder);
        createResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var createdOrderJson = await createResponse.Content.ReadAsStringAsync();
        var createdOrder = System.Text.Json.JsonSerializer.Deserialize<System.Text.Json.JsonElement>(createdOrderJson);
        var orderId = createdOrder.GetProperty("order").GetProperty("id").GetString();

        // Then delete the order
        var deleteResponse = await _client.DeleteAsync($"/order/{orderId}");
        deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task DeleteOrder_NonExistentOrder_ReturnsNotFound()
    {
        var nonExistentId = Guid.NewGuid();
        var response = await _client.DeleteAsync($"/order/{nonExistentId}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    #endregion
}
