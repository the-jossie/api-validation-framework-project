using System.Diagnostics;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();

        var method = context.Request.Method;
        var path = context.Request.Path.ToString();
        var userAgent = context.Request.Headers.UserAgent.ToString();
        var remoteIp = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        var traceId = context.TraceIdentifier;

        try
        {
            await _next(context);
        }
        finally
        {
            stopwatch.Stop();
            var statusCode = context.Response?.StatusCode;

            _logger.LogInformation(
                "HTTP {Method} {Path} responded {StatusCode} in {ElapsedMs} ms | UA={UserAgent} | IP={RemoteIp} | TraceId={TraceId}",
                method,
                path,
                statusCode,
                stopwatch.Elapsed.TotalMilliseconds,
                userAgent,
                remoteIp,
                traceId
            );
        }
    }
}

