namespace TrainsOnline.Application
{
    using System.Reflection;
    using AutoMapper;
    using MediatR;
    using Microsoft.Extensions.DependencyInjection;

    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationContent(this IServiceCollection services)
        {
            services.AddAutoMapper(new Assembly[] { typeof(DependencyInjection).GetTypeInfo().Assembly }, serviceLifetime: ServiceLifetime.Singleton);
            services.AddMediatR(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
