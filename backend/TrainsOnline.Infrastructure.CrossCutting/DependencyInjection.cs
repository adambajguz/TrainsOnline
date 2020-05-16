namespace TrainsOnline.Infrastructure.CrossCutting
{
    using Microsoft.Extensions.DependencyInjection;
    using TrainsOnline.Common.Interfaces;
    using TrainsOnline.Infrastructure.CrossCutting.MachineDateTime;

    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureCrossCuttingLayer(this IServiceCollection services)
        {
            services.AddSingleton<IMachineDateTimeService, MachineDateTimeService>();

            return services;
        }
    }
}
