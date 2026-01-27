using ProPlan.Entities.ErrorModel;
using ProPlan.Entities.Exceptions;

namespace ProPlan.WebApi.Middleware
{
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
            catch (NotFoundException notFoundEx)
            {
                _logger.LogWarning(notFoundEx, "Resource not found.");
                await HandleExceptionAsync(context, notFoundEx, StatusCodes.Status404NotFound, notFoundEx.Message);
            }
            catch (BadRequestException badRequestEx)
            {
                _logger.LogWarning(badRequestEx, "Bad request.");
                await HandleExceptionAsync(context, badRequestEx, StatusCodes.Status400BadRequest, badRequestEx.Message);
            }
            catch (ValidationException validationEx)
            {
                _logger.LogWarning(validationEx, "Validation error.");
                await HandleValidationExceptionAsync(context, validationEx);
            }
            catch (UnauthorizedException unauthorizedEx)
            {
                _logger.LogWarning(unauthorizedEx, "Unauthorized access.");
                await HandleExceptionAsync(context, unauthorizedEx, StatusCodes.Status401Unauthorized, unauthorizedEx.Message);
            }
            catch (ConflictException conflictEx)
            {
                _logger.LogWarning(conflictEx, "Conflict occurred.");
                await HandleExceptionAsync(context, conflictEx, StatusCodes.Status409Conflict, conflictEx.Message);
            }
            catch (ArgumentException argEx)
            {
                _logger.LogError(argEx, "Argument exception occurred.");
                await HandleExceptionAsync(context, argEx, StatusCodes.Status400BadRequest, "Geçersiz parametre.");
            }
            catch (KeyNotFoundException keyEx)
            {
                _logger.LogError(keyEx, "Resource not found.");
                await HandleExceptionAsync(context, keyEx, StatusCodes.Status404NotFound, "Kaynak bulunamadı.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Bilinmeyen bir hata oluştu.");
                await HandleExceptionAsync(context, ex, StatusCodes.Status500InternalServerError, "Beklenmedik bir hata oluştu.");
            }
        }
        private async Task HandleExceptionAsync(HttpContext context, Exception exception, int statusCode, string message)
        {
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";

            var errorResponse = ErrorResponse.Create(statusCode, message);

            // Sadece geliştirme ortamında stack trace gösteriyoruz.
            if (context.RequestServices.GetService(typeof(IHostEnvironment)) is IHostEnvironment env && env.IsDevelopment())
            {
                errorResponse.Details = exception.StackTrace;
            }

            await context.Response.WriteAsJsonAsync(errorResponse);
        }
        private async Task HandleValidationExceptionAsync(HttpContext context, ValidationException exception)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Response.ContentType = "application/json";

            var errorResponse = ErrorResponse.Create(
                StatusCodes.Status400BadRequest,
                exception.Message,
                exception.Errors
            );

            await context.Response.WriteAsJsonAsync(errorResponse);
        }
    }
}
