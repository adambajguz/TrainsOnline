namespace TrainsOnline.Infrastructure
{
    using System.Linq;
    using Application.Interfaces;
    using Infrastructure.DataRights;
    using Infrastructure.UoW;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.IdentityModel.Tokens;
    using Serilog;
    using TrainsOnline.Application.Interfaces.Documents;
    using TrainsOnline.Application.Interfaces.UoW;
    using TrainsOnline.Application.Interfaces.UoW.Generic;
    using TrainsOnline.Infrastructure.CurrentUser;
    using TrainsOnline.Infrastructure.Documents;
    using TrainsOnline.Infrastructure.Jwt;
    using TrainsOnline.Infrastructure.Main.Email;
    using TrainsOnline.Infrastructure.QRCode;
    using TrainsOnline.Infrastructure.StringSimilarityComparer;
    using TrainsOnline.Infrastructure.UserManager;
    using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IJwtService, JwtService>();
            services.AddSingleton<IEmailService, EmailService>();
            services.AddSingleton<IUserManagerService, UserManagerService>();
            services.AddSingleton<IDocumentsService, DocumentsService>();
            services.AddSingleton<IQRCodeService, QRCodeService>();
            services.AddSingleton<IStringSimilarityComparerService, StringSimilarityComparerService>();

            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddScoped<IPKPAppDbUnitOfWork, PKPAppDbUnitOfWork>();
            services.AddScoped<ITrainsOnlineMongoUnitOfWork, TrainsOnlineUnitOfWork>();
            services.AddScoped<IDataRightsService, DataRightsService>();

            // Set license key to use GemBox.Document in Free mode.
            GemBox.Document.ComponentInfo.SetLicense("FREE-LIMITED-KEY");
            GemBox.Document.ComponentInfo.FreeLimitReached += ComponentInfo_FreeLimitReached;

            //email configruation
            {
                IConfigurationSection emailSettings = configuration.GetSection("EmailSettings");
                services.Configure<EmailSettings>(emailSettings);
            }

            //jwt authentication configuration
            {
                IConfigurationSection jwtSettingsSection = configuration.GetSection("JwtSettings");
                services.Configure<JwtSettings>(jwtSettingsSection);

                JwtSettings jwtSettings = jwtSettingsSection.Get<JwtSettings>();
                byte[] key = Base64UrlEncoder.DecodeBytes(jwtSettings.Key);
                services.AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = true;
                    x.SaveToken = true;
                    x.TokenValidationParameters = JwtService.GetValidationParameters(key);
                });
            }

            return services;
        }

        private static void ComponentInfo_FreeLimitReached(object? sender, GemBox.Document.FreeLimitEventArgs e)
        {
            int numberOfParagraphs = e.Document.GetChildElements(true, GemBox.Document.ElementType.Paragraph)
                                               .Count();

            Log.Error("Free limit of paragraphs reached in GemBox ({numberOfParagraphs})", numberOfParagraphs);
        }
    }
}
