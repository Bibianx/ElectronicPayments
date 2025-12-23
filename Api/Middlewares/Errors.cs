using System.Net;
using System.Text.Json;

namespace Api.Middlewares
{
    public class ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
    {
        private readonly ILogger<ErrorHandlingMiddleware> _logger = logger;
        private readonly RequestDelegate _next = next;

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception");

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";

                var response_api = new
                {
                    success = false,
                    message = "Ha ocurrido un error inesperado en la operacion",
                    error = ex.Message
                };

                await context.Response.WriteAsync(JsonSerializer.Serialize(response_api));
            }
        }
    }
}