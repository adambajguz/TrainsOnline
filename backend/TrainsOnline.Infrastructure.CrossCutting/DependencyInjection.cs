namespace TrainsOnline.Infrastructure.CrossCutting
{
    using Microsoft.Extensions.DependencyInjection;
    using TrainsOnline.Common.Interfaces;
    using TrainsOnline.Infrastructure.Cache;
    using TrainsOnline.Infrastructure.CrossCutting.MachineDateTime;

    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureCrossCuttingLayer(this IServiceCollection services)
        {
            services.AddSingleton<IMachineInfoService, MachineInfoService>();
            services.AddSingleton<ICachingService, CachingService>();

            return services;
        }
    }
}
