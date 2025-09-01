using Microsoft.EntityFrameworkCore;
using System.Text;

public class ForensicLoggingMiddleware
{
    private readonly RequestDelegate _next;

    public ForensicLoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, AppDbContext db)
    {
        var requestBody = "";
        if (context.Request.ContentLength > 0 && context.Request.Body.CanRead)
        {
            context.Request.EnableBuffering();
            using var reader = new StreamReader(context.Request.Body, Encoding.UTF8, leaveOpen: true);
            requestBody = await reader.ReadToEndAsync();
            context.Request.Body.Position = 0;
        }

        var requestHeaders = string.Join("\n",
            context.Request.Headers.Select(h => $"{h.Key}: {h.Value}"));

        var originalBodyStream = context.Response.Body;
        using var responseBodyStream = new MemoryStream();
        context.Response.Body = responseBodyStream;

        try
        {
            await _next(context); // let pipeline run
        }
        catch (Exception ex)
        {
            // capture exception details as synthetic response
            context.Response.StatusCode = 500;
            await responseBodyStream.WriteAsync(Encoding.UTF8.GetBytes(ex.ToString()));
        }

        responseBodyStream.Seek(0, SeekOrigin.Begin);
        var responseBody = await new StreamReader(responseBodyStream).ReadToEndAsync();
        responseBodyStream.Seek(0, SeekOrigin.Begin);

        var responseHeaders = string.Join("\n",
            context.Response.Headers.Select(h => $"{h.Key}: {h.Value}"));

        var log = new ForensicLog
        {
            Method = context.Request.Method,
            Path = context.Request.Path,
            QueryString = context.Request.QueryString.ToString(),
            RequestHeaders = requestHeaders,
            RequestBody = requestBody,
            ResponseStatusCode = context.Response.StatusCode,
            ResponseHeaders = responseHeaders,
            ResponseBody = responseBody,
            RemoteIp = context.Connection.RemoteIpAddress?.ToString(),
            UserAgent = context.Request.Headers["User-Agent"].ToString(),
            UserId = context.User.Identity?.Name ?? "anonymous",
            Timestamp = DateTime.UtcNow
        };

        db.ForensicLogs.Add(log);
        await db.SaveChangesAsync();

        await responseBodyStream.CopyToAsync(originalBodyStream);
    }
}
