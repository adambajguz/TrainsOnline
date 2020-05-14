namespace TrainsOnline.Api.CustomMiddlewares.Exceptions
{
    using System;
    using System.Net;
    using System.Threading.Tasks;
    using Application.Exceptions;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Newtonsoft.Json;
    using Serilog;
    using TrainsOnline.Api.CustomMiddlewares.Exceptions.ValidationFormatter;

    public class CustomExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomExceptionHandlerMiddleware(RequestDelegate next)
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
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            object errorObject = new object();
            HttpStatusCode code = exception switch
            {
                FluentValidation.ValidationException ex => HandleValidationException(ex, ref errorObject),
                BadUserException _ => HttpStatusCode.Forbidden,
                ForbiddenException _ => HttpStatusCode.Unauthorized,
                NotFoundException _ => HttpStatusCode.NotFound,
                _ => HandleUnknownException(exception)
            };

            ExceptionResponse response = new ExceptionResponse(code, exception.Message, errorObject);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
        }

        private static HttpStatusCode HandleValidationException(FluentValidation.ValidationException exception, ref object errors)
        {
            errors = exception.FormatValidationExceptions();

            return HttpStatusCode.BadRequest;
        }

        private static HttpStatusCode HandleUnknownException(Exception exception)
        {
            Log.Error(exception, "Unhandled exception in CustomExceptionHandlerMiddleware");

            return HttpStatusCode.InternalServerError;
        }
    }

    public static class CustomExceptionHandlerMiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CustomExceptionHandlerMiddleware>();
        }
    }
}
