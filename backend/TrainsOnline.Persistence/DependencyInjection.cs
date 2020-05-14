namespace TrainsOnline.Persistence
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using TrainsOnline.Application.Interfaces;
    using TrainsOnline.Common;
    using TrainsOnline.Persistence.DbContext;
    using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistenceContent(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<PKPAppDbContext>(options => options.UseSqlServer(configuration.GetConnectionString(GlobalAppConfig.PKPAPP_DB_CONNECTION_STRING_NAME)))
                    .AddScoped<IPKPAppDbContext>(c => c.GetRequiredService<PKPAppDbContext>());

            return services;
        }
    }
}
