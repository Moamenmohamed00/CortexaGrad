using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Cortexa.Api.Middlewares
{
    /// <summary>
    /// Global exception-handling middleware. Catches unhandled exceptions,
    /// logs them, and returns a standardized ProblemDetails JSON response.
    /// </summary>
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
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

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var (statusCode, title) = exception switch
            {
                ValidationException => (StatusCodes.Status400BadRequest, "Validation Error"),
                InvalidOperationException => (StatusCodes.Status400BadRequest, "Bad Request"),
                KeyNotFoundException => (StatusCodes.Status404NotFound, "Resource Not Found"),
                UnauthorizedAccessException => (StatusCodes.Status401Unauthorized, "Unauthorized"),
                _ when IsNotFoundException(exception)
                                           => (StatusCodes.Status404NotFound, "Resource Not Found"),
                _ when IsConflictException(exception)
                                           => (StatusCodes.Status409Conflict, "Conflict"),
                _ => (StatusCodes.Status500InternalServerError, "Server Error")
            };

            if (statusCode == StatusCodes.Status500InternalServerError)
            {
                _logger.LogError(exception, "Unhandled exception occurred: {Message}", exception.Message);
            }
            else
            {
                _logger.LogWarning(exception, "Handled exception ({StatusCode}): {Message}", statusCode, exception.Message);
            }

            var problemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = title,
                Detail = statusCode == StatusCodes.Status500InternalServerError
                    ? "An unexpected error occurred. Please try again later."
                    : exception.Message,
                Instance = context.Request.Path
            };

            // Attach validation errors as an extension property
            if (exception is ValidationException validationException)
            {
                problemDetails.Extensions["errors"] = validationException.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => e.ErrorMessage).ToArray());
            }

            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/problem+json";

            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            await context.Response.WriteAsJsonAsync(problemDetails, options);
        }

        /// <summary>
        /// Checks if the exception type name contains "NotFound" (convention-based matching
        /// for domain exceptions like PatientNotFoundException).
        /// </summary>
        private static bool IsNotFoundException(Exception ex) =>
            ex.GetType().Name.Contains("NotFound", StringComparison.OrdinalIgnoreCase);

        /// <summary>
        /// Checks if the exception type name contains "NotAvailable" or "Conflict"
        /// (convention-based matching for domain exceptions like BedNotAvailableException).
        /// </summary>
        private static bool IsConflictException(Exception ex) =>
            ex.GetType().Name.Contains("NotAvailable", StringComparison.OrdinalIgnoreCase) ||
            ex.GetType().Name.Contains("Conflict", StringComparison.OrdinalIgnoreCase);
    }
}
