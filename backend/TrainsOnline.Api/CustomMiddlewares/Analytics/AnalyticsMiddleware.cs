namespace TrainsOnline.Api.CustomMiddlewares.Analytics
{
    using System;
    using System.Net;
    using System.Threading.Tasks;
    using MediatR;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Primitives;
    using Microsoft.Net.Http.Headers;
    using Serilog;
    using TrainsOnline.Application.DTO;
    using TrainsOnline.Application.Handlers.AnalyticsRecordHandlers.Commands.CreateOrUpdateAnalyticsRecord;

    public class AnalyticsMiddleware
    {
        private readonly RequestDelegate _next;

        public AnalyticsMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                PathString path = context.Request.Path;
                StringValues referer = context.Request.Headers[HeaderNames.Referer];
                IPAddress ip = context.Connection.RemoteIpAddress;
                StringValues userAgent = context.Request.Headers[HeaderNames.UserAgent];

                IMediator? mediator = context.RequestServices.GetService<IMediator>();
                if (!(mediator is null))
                {
                    var data = new CreateOrUpdateAnalyticsRecordRequest
                    {
                        Uri = path.ToString(),
                        UserAgent = userAgent.ToString(),
                        Ip = ip.ToString()
                    };

                    IdResponse id = await mediator.Send(new CreateOrUpdateAnalyticsRecordCommand(data));
                    Log.Debug("Created or updated analytics record with id {Id}", id.Id);
                }

                Log.Debug("Request to {Path} {Referer} from {IP} and {UserAgent}", path, referer, ip, userAgent);
            }
            catch (Exception ex)
            {
                Log.Error($"Error in analytics middleware", ex);
            }

            await _next(context);
        }
    }

    public static class AnalyticsMiddlewareExtensions
    {
        public static IApplicationBuilder UseAnalytics(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AnalyticsMiddleware>();
        }
    }
}
