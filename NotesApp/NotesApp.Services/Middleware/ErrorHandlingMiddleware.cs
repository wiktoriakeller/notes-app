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
            catch (UnauthenticatedException e)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            }
            catch (ForbiddenException e)
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
            }
            catch(NotFoundException e)
            {
                context.Response.StatusCode = StatusCodes.Status404NotFound;
            }
            catch(ExpiredException e)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
            }
            catch(BadRequestException e)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
            }
            catch(InternalServerErrorException e)
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            }
            catch (Exception)
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            }
        }
    }
}
