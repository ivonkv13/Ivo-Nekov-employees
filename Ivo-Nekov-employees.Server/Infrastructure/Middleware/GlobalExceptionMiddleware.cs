using System.Net;
using System.Text.Json;

namespace Ivo_Nekov_employees.Server.Infrastructure.Middleware
{
    /// <summary>
    /// Can be useful in the future when we need logging or tracking unexpected errors across the application.
    /// </summary>
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                var response = context.Response;
                response.ContentType = "application/json";


                //This order needs to be enforced/used in order to reach filenotfound and directorynotfound exceptions
                response.StatusCode = ex switch
                {
                    FileNotFoundException => (int)HttpStatusCode.NotFound,
                    DirectoryNotFoundException => (int)HttpStatusCode.NotFound,
                    NotSupportedException => (int)HttpStatusCode.BadRequest,
                    JsonException => (int)HttpStatusCode.BadRequest,
                    InvalidOperationException => (int)HttpStatusCode.BadRequest,
                    IOException => (int)HttpStatusCode.InternalServerError,
                    _ => (int)HttpStatusCode.InternalServerError
                };

                var errorResponse = new
                {
                    Error = ex.GetType().Name,
                    Message = ex.Message
                };

                await response.WriteAsync(JsonSerializer.Serialize(errorResponse));
            }
        }
    }
}
