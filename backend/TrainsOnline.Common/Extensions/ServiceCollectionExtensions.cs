namespace TrainsOnline.Common.Extensions
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddConfiguration<TOptions>(this IServiceCollection services, IConfiguration configuration, string? overrideFefauktSectionName = null)
            where TOptions : class
        {
            IConfigurationSection settingsSection = configuration.GetSection(nameof(TOptions));
            services.Configure<TOptions>(settingsSection);

            return services;
        }
    }
}
