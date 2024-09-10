using Ardalis.GuardClauses;
using TaskList.Domain.Exceptions;

namespace TaskList.Web.Infrastructure
{
    public class ExceptionHandlingMiddleware
    {
        private readonly Dictionary<Type, Func<HttpContext, Exception, string, Task>> _exceptionHandlers;

        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _exceptionHandlers = new()
            {
                { typeof(NotFoundException), HandleNotFoundErrorAsync },
                { typeof(ForbiddenException), HandleForbiddenErrorAsync },
                { typeof(Exception), HandleServerErrorAsync },
            };

            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {

                if(_exceptionHandlers.ContainsKey(ex.GetType()))
                {
                    await _exceptionHandlers[ex.GetType()]
                        .Invoke(context, ex, Guid.NewGuid().ToString());
                }
                else
                {
                    await _exceptionHandlers[typeof(Exception)]
                        .Invoke(context, ex, Guid.NewGuid().ToString());
                }
            }
        }
        private Task HandleServerErrorAsync(HttpContext context, Exception exception, string traceId)
        {
            context.Response.StatusCode = 500;

            return context.Response.WriteAsJsonAsync(new
            {
                Status = StatusCodes.Status500InternalServerError,
                TraceId = traceId,
                Title = "An error occurred on the server.",
                Detail = exception.Message,
            });
        }

        private Task HandleNotFoundErrorAsync(HttpContext context, Exception exception, string traceId)
        {
            context.Response.StatusCode = 404;

            return context.Response.WriteAsJsonAsync(new
            {
                Status = StatusCodes.Status404NotFound,
                TraceId = traceId,
                Title = "The specified resource was not found.",
                Detail = exception.Message
            });
        }

        private Task HandleForbiddenErrorAsync(HttpContext context, Exception exception, string traceId)
        {
            context.Response.StatusCode = 403;

            return context.Response.WriteAsJsonAsync(new
            {
                Status = StatusCodes.Status403Forbidden,
                TraceId = traceId,
                Title = "Forbidden",
                Detail = exception.Message
            });
        }
    }
}
