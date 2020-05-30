namespace TrainsOnline.Api.CustomMiddlewares.Analytics
{
    using System;
    using System.Net;
    using System.Text.RegularExpressions;
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
        private const int MAX_PATH_LENGTH = 256;

        private static readonly Regex regex = new Regex(@"[({]?[a-fA-F0-9]{8}[-]?([a-fA-F0-9]{4}[-]?){3}[a-fA-F0-9]{12}[})]?", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.CultureInvariant);

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
                IPAddress ip = context.Connection.RemoteIpAddress;
                StringValues userAgent = context.Request.Headers[HeaderNames.UserAgent];

                string sanitizedPath = SanitizeUrl(path.Value);

                IMediator? mediator = context.RequestServices.GetService<IMediator>();
                if (!(mediator is null))
                {
                    CreateOrUpdateAnalyticsRecordRequest data = new CreateOrUpdateAnalyticsRecordRequest
                    {
                        Uri = sanitizedPath,
                        UserAgent = userAgent.ToString(),
                        Ip = ip.ToString()
                    };

                    IdResponse id = await mediator.Send(new CreateOrUpdateAnalyticsRecordCommand(data));
                    Log.Debug("Created or updated analytics record with id {Id}", id.Id);
                }

                Log.Debug("Request to {Path} from {IP} and {UserAgent}", sanitizedPath, ip, userAgent);
            }
            catch (Exception ex)
            {
                Log.Error($"Error in analytics middleware", ex);
            }

            await _next(context);
        }

        private static string SanitizeUrl(string url)
        {
            string[] splitUrl = url.Split(new char[] { '?', '#' });
            string cleanedUrl = splitUrl[0];

            cleanedUrl = regex.Replace(cleanedUrl, "${__UUID}");

            bool needsTrimming = cleanedUrl.Length > MAX_PATH_LENGTH;
            if (needsTrimming)
            {
                cleanedUrl.Substring(0, MAX_PATH_LENGTH);
                cleanedUrl += "${__TRM}";
            }

            return cleanedUrl + "${__Q}";
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
