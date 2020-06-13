namespace TrainsOnline.Api.CustomMiddlewares.Exceptions
{
    using System;
    using System.Net;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Diagnostics;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Newtonsoft.Json;
    using Serilog;
    using TrainsOnline.Api.CustomMiddlewares.Exceptions.ValidationFormatter;
    using TrainsOnline.Application.Exceptions;
    using TrainsOnline.Common;

    public static class CustomExceptionHandler
    {
        public static void UseCustomErrors(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (GlobalAppConfig.DEV_MODE)//env.IsDevelopment())
                app.Use(WriteDevelopmentResponse);
            else
                app.Use(WriteProductionResponse);
        }

        private static Task WriteDevelopmentResponse(HttpContext context, Func<Task> next)
        {
            return WriteResponse(context, includeDevDetails: true);
        }

        private static Task WriteProductionResponse(HttpContext context, Func<Task> next)
        {
            return WriteResponse(context, includeDevDetails: false);
        }

        private static async Task WriteResponse(HttpContext context, bool includeDevDetails)
        {
            // Try and retrieve the error from the ExceptionHandler middleware
            IExceptionHandlerFeature? exceptionDetails = context.Features.Get<IExceptionHandlerFeature>();
            Exception? ex = exceptionDetails?.Error;

            await HandleExceptionAsync(context, ex, includeDevDetails);
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception? exception, bool includeDevDetails)
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

            string message = exception?.Message ?? "Unknown exception";

            if (includeDevDetails)
                message += " ~ StackTrace: " + exception?.StackTrace ?? "???";

            ExceptionResponse response = new ExceptionResponse(code, message, errorObject);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;

            string responseJson = JsonConvert.SerializeObject(response);
            await context.Response.WriteAsync(responseJson);
        }

        private static HttpStatusCode HandleValidationException(FluentValidation.ValidationException exception, ref object errors)
        {
            errors = exception.FormatValidationExceptions();

            return HttpStatusCode.BadRequest;
        }

        private static HttpStatusCode HandleUnknownException(Exception? exception)
        {
            Log.Error(exception, "Unhandled exception in CustomExceptionHandlerMiddleware");

            return HttpStatusCode.InternalServerError;
        }
    }
}