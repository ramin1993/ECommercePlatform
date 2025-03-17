using System.Net;
using System.Text.Json;
using ProductService.Services.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
namespace ProductService.Services.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context) //Instead of Invoke use InvokeAsync 
        {
            try
            {
                await _next(context);
            }
            catch (CustomException ex)
            {
                _logger.LogWarning($"Handled Custom Exception: {ex.Message}");
                await HandleCustomExceptionAsync(context, ex, HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unhandled Exception: {ex}");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleCustomExceptionAsync(HttpContext context, Exception exception, HttpStatusCode statusCode)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;
            var response = new
            {
                StatusCode = (int)statusCode,
                Message = exception.Message
            };

            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = new
            {
                StatusCode = context.Response.StatusCode,
                Message = "An unexpected error occurred.",
                Details = exception.Message
            };

            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}