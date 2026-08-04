using System.Net;
using System.Text.Json;

namespace RadioApi.Middleware;


public class RadioApiExceptionMiddleware(RequestDelegate next, ILogger<RadioApiExceptionMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (HttpRequestException ex)
        {
            logger.LogError(ex, "An error occurred while calling the Radio Browser API.");
            await HandleExceptionAsync(context, ex, HttpStatusCode.BadGateway, "External API error.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An unexpected error occurred.");
            await HandleExceptionAsync(context, ex, HttpStatusCode.InternalServerError, "Internal server error.");
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception, HttpStatusCode code, string message)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)code;

        var result = JsonSerializer.Serialize(new
        {
            error = message,
            details = exception.Message
        });

        return context.Response.WriteAsync(result);
    }
}