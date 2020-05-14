namespace TrainsOnline.Api.SpecialPages
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.DependencyInjection;
    using TrainsOnline.Api.SoapEndpoints.Core;
    using TrainsOnline.Api.SpecialPages.Core;
    using TrainsOnline.Common;
    using TrainsOnline.Common.Extensions;

    public class SoapEndpointsPage : SpecialPage
    {
        public override string Route { get; } = ConfigureEndpoints.BaseUrl;

        public SoapEndpointsPage()
        {

        }

        public override async Task Render(HttpContext httpContext, IWebHostEnvironment environment, IServiceCollection services)
        {
            bool isDevelopment = GlobalAppConfig.DEV_MODE;

            IEnumerable<EndpointServiceSpecification> specifications = Assembly.GetExecutingAssembly()
                                                                               .GetAllSoapEndpointServicesSpecification();

            StringBuilder sb = new StringBuilder();
            sb.BeginHTML();

            sb.Append("<p>");
            sb.Append(GlobalAppConfig.AppInfo.AppDescriptionHTML);
            sb.Append("</p>");

            sb.Append("<h1>Soap Services Endpoints List</h1>");
            sb.Append($"<table border=\"{isDevelopment.ToChar()}\"><thead>");
            sb.Append("<tr>");
            sb.Append("<th>Endpoint</th>");
            sb.Append("<th>Name</th>");
            sb.Append("<th>Description</th>");

            if (isDevelopment)
            {
                sb.Append("<th>Route scheme</th>");
                sb.Append("<th>Implementation type</th>");
                sb.Append("<th>Service type</th>");
                sb.Append("<th>Lifetime</th>");
            }

            sb.Append("</tr>");
            sb.Append("</thead><tbody>");

            foreach (EndpointServiceSpecification spec in specifications)
            {
                string route = spec.ResolveRoute(ConfigureEndpoints.BaseUrl);

                sb.Append("<tr>");
                sb.Append($"<td><a href=\"{route}\">{route}</a></td>");
                sb.Append($"<td>{spec.RouteAttribute.Name}</td>");
                sb.Append($"<td>{spec.RouteAttribute.Description}</td>");

                if (isDevelopment)
                {
                    sb.Append($"<td>{spec.RouteAttribute.RouteScheme}</td>");
                    sb.Append($"<td>{spec.ImplementationType.FullName}</td>");
                    sb.Append($"<td>{spec.ServiceType.FullName}</td>");

                    ServiceDescriptor service = services.Where(x => x.ServiceType == spec.ServiceType).Single();
                    sb.Append($"<td>{service.Lifetime}</td>");
                }

                sb.Append("</tr>");
            }

            sb.Append("</tbody></table>");
            sb.EndHTML();

            await httpContext.Response.WriteAsync(sb.ToString());
        }
    }
}
