namespace TrainsOnline.Api
{
    using System.Net.Mime;
    using System.Threading.Tasks;
    using Application;
    using Infrastructure;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Diagnostics;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.HttpOverrides;
    using Microsoft.AspNetCore.WebUtilities;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Persistence;
    using Serilog;
    using TrainsOnline.Api.Configuration;
    using TrainsOnline.Api.CustomMiddlewares.Analytics;
    using TrainsOnline.Api.CustomMiddlewares.Exceptions;
    using TrainsOnline.Api.SpecialPages.Core;
    using TrainsOnline.Common;
    using TrainsOnline.Infrastructure.CrossCutting;
    using TrainsOnline.Persistence.DbContext;

    //TODO add api key
    public class Startup
    {
        private IServiceCollection? _services;

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddAutoMapper(cfg =>
            //    {
            //        cfg.AddProfile(new AutoMapperProfile(typeof(Application.Content.DependencyInjection).GetTypeInfo().Assembly));
            //    },
            //    new Assembly[] {
            //        typeof(Application.Content.DependencyInjection).GetTypeInfo().Assembly
            //    },
            //    serviceLifetime: ServiceLifetime.Singleton);

            //services.AddMediatR(new Assembly[] {
            //    typeof(Application.Content.DependencyInjection).GetTypeInfo().Assembly
            //    });

            services.AddRestApi()
                    .AddInfrastructureCrossCuttingLayer()
                    .AddInfrastructureLayer(Configuration)
                    .AddPersistenceLayer(Configuration)
                    .AddApplicationLayer();

            services.AddHealthChecks()
                    .AddDbContextCheck<TrainsOnlineRelationalDbContext>();
            //.AddMongoDb();

            _services = services;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            if (GlobalAppConfig.DEV_MODE)
            {
                app.UseDeveloperExceptionPage();
                //app.UseBrowserLink();
            }
            else
            {
                if (!FeaturesSettings.AlwaysUseExceptionHandling)
                    app.UseExceptionHandler(error => error.UseCustomErrors(env));

                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            if (FeaturesSettings.AlwaysUseExceptionHandling)
                app.UseExceptionHandler(error => error.UseCustomErrors(env));

            app.UseHttpsRedirection();

            //app.UseStaticFiles()

            app.UseRouting()
               .UseResponseCompression()
               .UseCors("AllowAll")
               .UseStatusCodePages(StatusCodePageRespone);

            app.UseAuthentication()
               .UseAuthorization();
            //.UseSession();

            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapHealthChecks("/healthy");
                endpoints.MapControllers();
            });
            app.ConfigureSpecialPages(Environment, _services)
               .UseHealthChecks(GlobalAppConfig.AppInfo.HealthUrl);

            app.ConfigureSwagger()
               .UseAnalytics()
               .UseSerilogRequestLogging();
        }

        private static async Task StatusCodePageRespone(StatusCodeContext statusCodeContext)
        {
            HttpResponse httpRespone = statusCodeContext.HttpContext.Response;
            httpRespone.ContentType = MediaTypeNames.Text.Plain;

            string reasonPhrase = ReasonPhrases.GetReasonPhrase(httpRespone.StatusCode);

            string response = $"{GlobalAppConfig.AppInfo.AppName} Error Page\n" +
                              $"{GlobalAppConfig.AppInfo.AppVersionText}\n\n" +
                              $"Status code: {httpRespone.StatusCode} - {reasonPhrase}";

            await httpRespone.WriteAsync(response);
        }
    }
}
