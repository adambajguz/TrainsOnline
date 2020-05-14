namespace TrainsOnline.Api.SoapEndpoints.Core
{
    using System.Reflection;
    using System.ServiceModel;
    using Microsoft.AspNetCore.Routing;
    using Microsoft.Extensions.DependencyInjection;
    using SoapCore;
    using SoapCore.Extensibility;
    using TrainsOnline.Api.CustomMiddlewares.Soap;
    using TrainsOnline.Application.Interfaces;
    using TrainsOnline.Common.Extensions;

    public static class ConfigureEndpoints
    {
        public static string BaseUrl { get; } = "/soap-api";

        public static IServiceCollection AddSoapApiServices(this IServiceCollection services)
        {
            //services.AddSoapCore();
            services.AddSingleton<IFaultExceptionTransformer, DefaultFaultExceptionTransformer<CustomMessage>>()
                    .AddSingleton<IOperationInvoker, SoapCoreSafeOperationInvoker>()
                    .AddSoapServiceOperationTuner(new SoapJwtMiddleware(services.BuildServiceProvider()
                                                                                .GetService<IJwtService>()));

            //services.AddTransient<ISampleSoapEndpointService, SampleSoapEndpointService>();

            Assembly.GetExecutingAssembly()
                    .GetAllSoapEndpointServicesSpecification()
                    .ForEach(x => services.AddTransient(x.ServiceType, x.ImplementationType));


            return services;
        }

        public static IEndpointRouteBuilder MapSoapServices(this IEndpointRouteBuilder routes)
        {
            //routes.UseSoapEndpoint<IAuthenticationSoapEndpointService>(AuthenticationEndpoint, new BasicHttpBinding(), SoapSerializer.DataContractSerializer);

            Assembly.GetExecutingAssembly()
                    .GetAllSoapEndpointServicesSpecification()
                    .ForEach(x => routes.UseSoapEndpoint(x.ServiceType,
                                                         x.ResolveRoute(BaseUrl),
                                                         new BasicHttpsBinding(),
                                                         SoapSerializer.XmlSerializer));

            return routes;
        }
    }
}
