namespace TrainsOnline.Persistence
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using TrainsOnline.Application.Interfaces;
    using TrainsOnline.Persistence.DbContext;

    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistenceLayer(this IServiceCollection services, IConfiguration configuration)
        {
            //database configruation
            {
                IConfigurationSection emailSettings = configuration.GetSection("DatabaseSettings");
                services.Configure<DatabaseSettings>(emailSettings);
            }

            services.AddSingleton<ITrainsOnlineDbContext, TrainsOnlineMongoDbContext>();

            return services;
        }
    }
}
