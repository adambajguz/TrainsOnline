namespace TrainsOnline.Persistence
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using TrainsOnline.Application.Interfaces;
    using TrainsOnline.Application.Interfaces.DbContext;
    using TrainsOnline.Persistence.DbContext;

    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistenceLayer(this IServiceCollection services, IConfiguration configuration)
        {
            //database configruation
            {
                IConfigurationSection databaseSettingsSection = configuration.GetSection("DatabaseSettings");
                services.Configure<DatabaseSettings>(databaseSettingsSection);
            }

            services.AddDbContext<PKPAppDbContext>(options => options.UseSqlServer(configuration.GetConnectionString(ConnectionStringsNames.SQLDatabase)))
                    .AddScoped<IPKPAppDbContext>(c => c.GetRequiredService<PKPAppDbContext>());


            services.AddSingleton<ITrainsOnlineMongoDbContext, TrainsOnlineMongoDbContext>();

            return services;
        }
    }
}
