namespace TrainsOnline.Persistence
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using TrainsOnline.Application.Interfaces;
    using TrainsOnline.Application.Interfaces.DbContext;
    using TrainsOnline.Common.Extensions;
    using TrainsOnline.Persistence.DbContext;

    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistenceLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddConfiguration<DatabaseSettings>(configuration);

            services.AddDbContext<TrainsOnlineRelationalDbContext>(options => options.UseSqlServer(configuration.GetConnectionString(ConnectionStringsNames.SQLDatabase)))
                    .AddScoped<ITrainsOnlineRelationalDbContext>(c => c.GetRequiredService<TrainsOnlineRelationalDbContext>());

            services.AddSingleton<ITrainsOnlineMongoDbContext, TrainsOnlineMongoDbContext>();

            return services;
        }
    }
}
