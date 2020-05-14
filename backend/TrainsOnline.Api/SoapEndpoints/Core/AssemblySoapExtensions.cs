namespace TrainsOnline.Api.SoapEndpoints.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public static class AssemblySoapExtensions
    {
        public static IEnumerable<Type> GetAllSoapEndpointServices(this Assembly assembly)
        {
            IEnumerable<Type> soapEndpointServices = from type in assembly.GetTypes()
                                                     where typeof(ISoapEndpointService).IsAssignableFrom(type) && type.IsClass && !type.IsAbstract
                                                     select type;

            return soapEndpointServices;
        }

        public static IEnumerable<EndpointServiceSpecification> GetAllSoapEndpointServicesSpecification(this Assembly assembly)
        {
            IEnumerable<Type> soapEndpointServices = assembly.GetAllSoapEndpointServices();

            IEnumerable<EndpointServiceSpecification> tmp = Enumerable.Select(soapEndpointServices, (type) =>
            {
                Type @interface = type.GetInterfaces()
                                    .Where(x => x.GetInterfaces()
                                                    .Contains(typeof(ISoapEndpointService)))
                                    .Single();

                SoapRouteAttribute soapRouteAttribute = type.GetCustomAttribute<SoapRouteAttribute>() ?? throw new NullReferenceException($"{nameof(SoapEndpoints.Core.SoapRouteAttribute)} not set in class {type.FullName}");

                return new EndpointServiceSpecification(type, @interface, soapRouteAttribute);
            });

            return tmp;
        }
    }

    public class EndpointServiceSpecification
    {
        public Type ImplementationType { get; }
        public Type ServiceType { get; }
        public SoapRouteAttribute RouteAttribute { get; }

        public EndpointServiceSpecification(Type implementation, Type @interface, SoapRouteAttribute routeAttribute)
        {
            ImplementationType = implementation;
            ServiceType = @interface;
            RouteAttribute = routeAttribute;
        }

        public string ResolveRoute(string basePath)
        {
            return RouteAttribute.RouteScheme.Replace("[baseUrl]", basePath);
        }
    }
}
