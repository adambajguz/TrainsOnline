namespace TrainsOnline.Api.Configuration
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.OpenApi.Models;
    using Serilog;
    using TrainsOnline.Common;

    public static class SwaggerConfiguration
    {
        public static IApplicationBuilder ConfigureSwagger(this IApplicationBuilder app)
        {
            app.UseSwagger(c =>
            {
                c.SerializeAsV2 = false;
                c.RouteTemplate = "api/{documentName}/swagger.json";
            });

            app.UseReDoc(c =>
            {
                c.RoutePrefix = GlobalAppConfig.AppInfo.ReDocRoute;
                c.SpecUrl(GlobalAppConfig.AppInfo.SwaggerStartupUrl);
            });

            app.UseSwaggerUI(c =>
            {
                c.DisplayRequestDuration();
                //c.EnableValidator();
                c.ShowExtensions();
                c.RoutePrefix = GlobalAppConfig.AppInfo.SwaggerRoute;
                c.SwaggerEndpoint(GlobalAppConfig.AppInfo.SwaggerStartupUrl, GlobalAppConfig.AppInfo.AppNameWithVersion);

                Assembly? assembly = MethodBase.GetCurrentMethod()?.DeclaringType?.Assembly;
                if (assembly is null)
                {
                    Log.Error("Custom SwaggerUI index.html cannot be retrived. Using default.");
                }
                else
                {
                    string? ns = assembly.GetName().Name;
                    string name = $"{ns}.index.html";

                    if (assembly.GetManifestResourceNames().Contains(name))
                    {
                        Log.Information("Custom SwaggerUI index.html stream will be used: {Name}.", name);
                        c.IndexStream = () => assembly.GetManifestResourceStream(name);
                    }
                    else
                    {
                        Log.Error("Custom SwaggerUI index.html stream is null for name: {Name}. Using default.", name);
                    }
                }
            });

            return app;
        }

        public static void AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.EnableAnnotations();
                c.SwaggerDoc(GlobalAppConfig.AppInfo.SwaggerDocumentName, new OpenApiInfo
                {
                    Version = GlobalAppConfig.AppInfo.AppVersionText,
                    Title = GlobalAppConfig.AppInfo.AppName,
                    Description = GlobalAppConfig.AppInfo.AppDescriptionHTML
                });

                c.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, //Name the security scheme
                    new OpenApiSecurityScheme
                    {
                        Description = "JWT Authorization header using the Bearer scheme. Use /api/login endpoint to retrive a token.",
                        Type = SecuritySchemeType.Http,
                        Scheme = "bearer"
                    });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Id = JwtBearerDefaults.AuthenticationScheme, //The name of the previously defined security scheme.
                                Type = ReferenceType.SecurityScheme
                            }
                        },
                        new List<string>()
                    }
                });
            });

            if (FeaturesSettings.UseNewtonsoftJson)
                services.AddSwaggerGenNewtonsoftSupport();
            else
                services.AddSwaggerGen();
        }
    }
}
