using Microsoft.OpenApi.Models;
using ApiValidationFramework.Services;

var builder = WebApplication.CreateBuilder(args);

var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ListenAnyIP(int.Parse(port));
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "API Validation Framework",
        Version = "v1"
    });
    c.UseInlineDefinitionsForEnums();
});
builder.Services.AddControllers();
builder.Services.AddSingleton<IOrderService, OrderService>();

var app = builder.Build();

app.UseMiddleware<RequestLoggingMiddleware>();
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseMiddleware<SemanticValidationMiddleware>();

app.MapControllers();

app.Run();

public partial class Program { }
