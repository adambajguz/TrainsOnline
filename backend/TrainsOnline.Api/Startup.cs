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

            services.AddInfrastructureCrossCuttingLayer()
                    .AddInfrastructureLayer(Configuration)
                    .AddPersistenceLayer(Configuration)
                    .AddApplicationLayer()
                    .AddRestApi();

            services.AddHealthChecks()
                    .AddDbContextCheck<PKPAppDbContext>();
            //.AddMongoDb();

            _services = services;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting()
               .UseSerilogRequestLogging()
               .UseCors("AllowAll")
               .UseCustomExceptionHandler()
               .UseResponseCompression()
               .UseStatusCodePages(StatusCodePageRespone)
               .UseHttpsRedirection();

            app.UseAuthentication()
               .UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapHealthChecks("/health");
                endpoints.MapControllers();
            });

            if (GlobalAppConfig.DEV_MODE)
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.ConfigureSpecialPages(Environment, _services);

            app.UseHealthChecks(GlobalAppConfig.AppInfo.HealthUrl);

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.ConfigureSwagger();
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
