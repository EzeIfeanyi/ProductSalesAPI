using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using ProductSalesAPI.Application.ApiUtilities.Shared;
using System.Net;
using System.Text.Json;

namespace ProductSalesAPI.Application.ApiUtilities.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred during the request.");
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        ApiResponse<string> response;

        if (exception is UnauthorizedAccessException || exception is SecurityTokenException)
        {
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            response = new ApiResponse<string>(false, "Unauthorized access", exception.Message);

            _logger.LogWarning(exception, "Unauthorized access attempted.");
        }
        else if (exception is InvalidOperationException)
        {
            context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
            response = new ApiResponse<string>(false, "Forbidden access", exception.Message);

            _logger.LogWarning(exception, "Forbidden access attempted.");
        }
        else
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            response = new ApiResponse<string>(false, "Internal Server Error", exception.Message);

            _logger.LogError(exception, "Internal server error occurred.");
        }

        return context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}
