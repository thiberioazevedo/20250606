using System.Text.Json;
using Thunders.TechTest.ApiService.DTOs;

namespace Thunders.TechTest.ApiService.Middlewares
{
    public class ValidationExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly JsonSerializerOptions _jsonOptions = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

        public ValidationExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                await HandleExceptionAsync(context, exception);
            }
        }

        Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var apiResponse = new ApiResponse { Success = false };

            switch (exception)
            {
                case FluentValidation.ValidationException validationException:
                    apiResponse.Message = validationException.Message;
                    apiResponse.Errors = validationException.Errors.Select(error => (ValidationErrorDetail)error);

                    break;

                default:
                    apiResponse.Message = GetLastException(exception).Message;
                    apiResponse.Errors = new List<ValidationErrorDetail>() { new() { Error = apiResponse.Message, Detail = apiResponse.Message } };
                    break;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status400BadRequest;

            return context.Response.WriteAsync(JsonSerializer.Serialize(apiResponse, _jsonOptions));
        }

        static Exception GetLastException(Exception exception)
        {
            var _exception = exception;

            while (_exception.InnerException != null)
            {
                _exception = _exception.InnerException;
            }

            return _exception;
        }
    }
}
