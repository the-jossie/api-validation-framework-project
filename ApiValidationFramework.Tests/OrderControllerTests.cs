using System.Net;
using System.Net.Http.Json;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;

public class OrderControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public OrderControllerTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

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

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
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

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
