namespace TrainsOnline.Api.SoapEndpoints.Core
{
    using System;

    [AttributeUsage(AttributeTargets.Class)]
    public class SoapRouteAttribute : Attribute
    {
        public string RouteScheme { get; }

        public string Name { get; }
        public string Description { get; }

        public SoapRouteAttribute(string routeScheme, string name, string description = "")
        {
            RouteScheme = routeScheme;
            Name = name;
            Description = description;
        }
    }
}
