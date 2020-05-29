namespace TrainsOnline.Api.CustomMiddlewares.Analytics
{
    using System;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
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
                StringBuilder info = new StringBuilder($"Got a request{Environment.NewLine}---{Environment.NewLine}");
                info.Append($"- remote IP: {context.Connection.RemoteIpAddress}{Environment.NewLine}");
                info.Append($"- path: {context.Request.Path}{Environment.NewLine}");
                info.Append($"- query string: {context.Request.QueryString}{Environment.NewLine}");
                info.Append($"- [headers] ua: {context.Request.Headers[HeaderNames.UserAgent]}{Environment.NewLine}");
                info.Append($"- [headers] referer: {context.Request.Headers[HeaderNames.Referer]}{Environment.NewLine}");

                Log.Debug(info.ToString());
            }
            catch (Exception ex)
            {
                // we don't care much about exceptions in analytics
                Log.Error($"Error in analytics middleware.", ex);
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
