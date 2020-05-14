namespace TrainsOnline.Api.SpecialPages.Core
{
    using System;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.DependencyInjection;
    using TrainsOnline.Api.SpecialPages;
    using TrainsOnline.Common;

    public static class SpecialPagesConfiguration
    {
        public static IApplicationBuilder ConfigureSpecialPages(this IApplicationBuilder app, IWebHostEnvironment environment, IServiceCollection? services)
        {
            if (services is null)
                throw new ArgumentNullException(nameof(services));

            // Register Development pages
            if (GlobalAppConfig.DEV_MODE)
                app.AddSpecialPage<RegisteredServicesPage>(environment, services);

            app.AddSpecialPage<SoapEndpointsPage>(environment, services);

            // Register Development and Production pages
            // Register Development and Production pages

            return app;
        }
    }
}
