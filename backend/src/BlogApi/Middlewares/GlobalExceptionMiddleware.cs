
using BlogApi.CustomExceptions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace BlogApi.Middlewares;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        _logger.LogError(exception, "An unhandled exception occurred.");

        if (exception is RequestException reqEx)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)reqEx.Code;

            return context.Response.WriteAsJsonAsync(new
            {
                Success = false,
                Message = reqEx.Message,
                Error = reqEx.Error
            });
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;

        var response = new
        {
            StatusCode = context.Response.StatusCode,
            Message = "Internal Server Error"
        };

        return context.Response.WriteAsJsonAsync(response);
    }
}