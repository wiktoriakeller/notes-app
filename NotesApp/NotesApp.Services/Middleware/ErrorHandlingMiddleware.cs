using Microsoft.AspNetCore.Http;
using NotesApp.Services.Exceptions;
using System.Text.Json;

namespace NotesApp.Services.Middleware
{
    public class ErrorHandlingMiddleware : IMiddleware
    {
        public ErrorHandlingMiddleware()
        {

        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var response = context.Response;
            response.ContentType = "application/json";

            try
            {
                await next.Invoke(context);
            }
            catch(Exception e)
            {
                var statusCode = GetStatusCode(e);
                response.StatusCode = statusCode;
                var result = JsonSerializer.Serialize( new { error = e?.Message } );
                await response.WriteAsync(result);
            }
        }

        private int GetStatusCode(Exception error) => error switch
        {
            UnauthenticatedException => StatusCodes.Status401Unauthorized,
            ForbiddenException => StatusCodes.Status403Forbidden,
            NotFoundException => StatusCodes.Status404NotFound,
            ExpiredException => StatusCodes.Status400BadRequest,
            BadHttpRequestException => StatusCodes.Status400BadRequest,
            InternalServerErrorException => StatusCodes.Status500InternalServerError,
            _ => StatusCodes.Status500InternalServerError
        };
    }
}
