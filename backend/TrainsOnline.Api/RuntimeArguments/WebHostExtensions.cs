namespace TrainsOnline.Api.RuntimeArguments
{
    using System;
    using FluentValidation;
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;
    using Serilog;
    using TrainsOnline.Api.Configuration;
    using TrainsOnline.Application.Interfaces;

    public static class WebHostExtensions
    {
        public static IWebHost RunWebHost()
        {
            SerilogConfiguration.ConfigureSerilog();

            Log.Information("Loading web host...");

            //Custom PropertyNameResolver to remove neasted Property in Classes e.g. Data.Id in UpdateUserCommandValidator.Model
            ValidatorOptions.PropertyNameResolver = (type, member, expression) =>
            {
                if (member != null)
                    return member.Name;

                return null;
            };


            ValidatorOptions.LanguageManager.Enabled = false;
            Log.Information("FluentValidation's support for localization disabled. Default English messages are forced to be used, regardless of the thread's CurrentUICulture.");
            //ValidatorOptions.LanguageManager.Culture = new CultureInfo("en");

            Log.Information("Starting web host...");

            return CreateWebHostBuilder().Build();
        }

        private static IWebHostBuilder CreateWebHostBuilder()
        {
            return WebHost.CreateDefaultBuilder()
                          .UseKestrel()
                          .UseStartup<Startup>()
                          .UseSerilog()
                          .UseUrls("http://*:2137", "http://*:2138");
        }

        public static void MigrateDatabase<TDbContext>(this IWebHost _) where TDbContext : IGenericMongoDatabaseContext
        {
            Console.WriteLine("This is app is not using Entity Framework Core. All done, closing app");
        }

        public static bool ValidateMigrations<TDbContext>(this IWebHost _) where TDbContext : IGenericMongoDatabaseContext
        {
            Console.WriteLine("This is app is not using Entity Framework Core. All done, closing app");

            return true;
        }
    }
}
