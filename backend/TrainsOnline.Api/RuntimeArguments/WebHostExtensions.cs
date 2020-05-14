namespace TrainsOnline.Api.RuntimeArguments
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using FluentValidation;
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
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

        public static void MigrateDatabase<TDbContext>(this IWebHost webHost) where TDbContext : IGenericDatabaseContext
        {
            Console.WriteLine($"Applying Entity Framework migrations for {typeof(TDbContext).Name}");

            IServiceScopeFactory serviceScopeFactory = (IServiceScopeFactory)webHost.Services.GetService(typeof(IServiceScopeFactory));

            using (IServiceScope scope = serviceScopeFactory.CreateScope())
            {
                TDbContext dbContext = scope.ServiceProvider.GetRequiredService<TDbContext>();

                dbContext.Database.Migrate();
                Console.WriteLine("All done, closing app");
            }
        }

        public static bool ValidateMigrations<TDbContext>(this IWebHost webHost) where TDbContext : IGenericDatabaseContext
        {
            Console.WriteLine($"Validating status of Entity Framework migrations for {typeof(TDbContext).Name}");

            IServiceScopeFactory serviceScopeFactory = (IServiceScopeFactory)webHost.Services.GetService(typeof(IServiceScopeFactory));

            using (IServiceScope scope = serviceScopeFactory.CreateScope())
            {
                TDbContext dbContext = scope.ServiceProvider.GetRequiredService<TDbContext>();

                IEnumerable<string> pendingMigrations = dbContext.Database.GetPendingMigrations();
                IList<string> migrations = pendingMigrations as IList<string> ?? pendingMigrations.ToList();
                if (!migrations.Any())
                {
                    Console.WriteLine("No pending migratons");
                    return true;
                }

                Console.WriteLine("Pending migratons {0}", migrations.Count());
                foreach (string migration in migrations)
                {
                    Console.WriteLine($"\t{migration}");
                }

                return false;
            }
        }
    }
}
