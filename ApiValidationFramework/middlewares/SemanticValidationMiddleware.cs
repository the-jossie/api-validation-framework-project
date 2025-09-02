using System.Text.Json;
using ApiValidationFramework.Dtos;

public class SemanticValidationMiddleware {
    private readonly RequestDelegate _next;
    public SemanticValidationMiddleware(RequestDelegate next) => _next = next;

    public async Task Invoke(HttpContext context) {
        // Handle POST and PUT requests for order validation
        if (context.Request.Path.StartsWithSegments("/order") &&
            (context.Request.Method == HttpMethods.Post || context.Request.Method == HttpMethods.Put)) {

            context.Request.EnableBuffering();
            var body = await new StreamReader(context.Request.Body).ReadToEndAsync();
            context.Request.Body.Position = 0;

            try
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var order = JsonSerializer.Deserialize<CreateOrderDto>(body, options);

                if (order?.EndDate < order?.StartDate)
                {
                    context.Response.StatusCode = 400;
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(JsonSerializer.Serialize(new
                    {
                        error = "EndDate must be after StartDate"
                    }));
                    return;
                }
            }
            catch (JsonException)
            {

            }
        }

        await _next(context);
    }
}
