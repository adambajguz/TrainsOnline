namespace TrainsOnline.Api.CustomMiddlewares.Analytics
{
    using System;
    using System.Net;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Primitives;
    using Microsoft.Net.Http.Headers;
    using Serilog;

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
