using System.Text.Json;

public class SemanticValidationMiddleware {
    private readonly RequestDelegate _next;
    public SemanticValidationMiddleware(RequestDelegate next) => _next = next;

    public async Task Invoke(HttpContext context) {
        if (context.Request.Path.StartsWithSegments("/order") &&
            context.Request.Method == HttpMethods.Post) {

            context.Request.EnableBuffering();
            var body = await new StreamReader(context.Request.Body).ReadToEndAsync();
            context.Request.Body.Position = 0;

            var order = JsonSerializer.Deserialize<Order>(body);

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

        await _next(context);
    }
}
