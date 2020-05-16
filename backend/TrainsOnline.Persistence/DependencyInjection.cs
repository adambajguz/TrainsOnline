namespace TrainsOnline.Persistence
{
    using Microsoft.Extensions.DependencyInjection;
    using TrainsOnline.Application.Interfaces;
    using TrainsOnline.Persistence.DbContext;

    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistenceLayer(this IServiceCollection services)
        {
            services.AddSingleton<ITrainsOnlineDbContext, TrainsOnlineDbContext>();

            return services;
        }
    }
}
