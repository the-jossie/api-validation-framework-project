using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using ApiValidationFramework.Services;

namespace ApiValidationFramework.Tests
{
    public class TestWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Ensure the OrderService is registered for testing
                services.AddSingleton<IOrderService, OrderService>();
            });
        }
    }
}
