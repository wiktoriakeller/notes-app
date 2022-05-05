using Microsoft.AspNetCore.Http;
using NotesApp.Services.Exceptions;

namespace NotesApp.Services.Middleware
{
    public class ErrorHandlingMiddleware : IMiddleware
    {
        public ErrorHandlingMiddleware()
        {

        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next.Invoke(context);
            }
            catch(ForbiddenException e)
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
            }
            catch(Exception e)
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            }
        }
    }
}
