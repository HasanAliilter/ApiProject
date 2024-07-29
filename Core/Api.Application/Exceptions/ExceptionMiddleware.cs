using FluentValidation;
using Microsoft.AspNetCore.Http;
using SendGrid.Helpers.Errors.Model;

namespace Api.Application.Exceptions
{
    public class ExceptionMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext httpContext, RequestDelegate next)
        {
            try
            {
                await next(httpContext);
            }
            catch (Exception ex)
            {
                await HandeExceptionAsync(httpContext, ex);
            }
        }

        private static Task HandeExceptionAsync(HttpContext httpContext, Exception exception)
        {
            int statusCode = GetStatusCode(exception);
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = statusCode;

            if(exception.GetType() == typeof(ValidationException))
            {
                return httpContext.Response.WriteAsync(new ExceptionModel
                {
                    Errors = ((ValidationException)exception).Errors.Select(x => x.ErrorMessage),
                    StatusCode = statusCode
                }.ToString());
            }


            List<string> errors = new()
            {
                $"Hata Mesajı : {exception.Message}",
            };

            return httpContext.Response.WriteAsync(new ExceptionModel
            {
                Errors = errors,
                StatusCode = statusCode
            }.ToString());
        }

        private static int GetStatusCode(Exception exception)
        {
            return exception switch
            {
                BadRequestException => StatusCodes.Status400BadRequest,
                NotFoundException => StatusCodes.Status400BadRequest,
                ValidationException => StatusCodes.Status422UnprocessableEntity,
                _ => StatusCodes.Status500InternalServerError
            };
        }

    }
}
