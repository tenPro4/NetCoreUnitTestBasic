using System.Net;

namespace Blog.Api.Infrastructure.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context /* other scoped dependencies */)
        {
            try
            {
                // must be awaited
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            // if it's not one of the expected exception, set it to 500
            var code = HttpStatusCode.InternalServerError;

            //TODO:Mapping if (exception is NotFoundExe) code = HttpStatusCode.NotFound;
            if (exception is ArgumentNullException) code = HttpStatusCode.BadRequest;
            else if (exception is HttpRequestException) code = HttpStatusCode.BadRequest;
            else if (exception is UnauthorizedAccessException) code = HttpStatusCode.Unauthorized;

            return Task.CompletedTask;
            //return WriteExceptionAsync(context, exception, code);
        }

        
    }
}
