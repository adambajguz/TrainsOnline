namespace TrainsOnline.Api.Configuration
{
    using System;
    using System.IO;
    using Microsoft.Extensions.Configuration;
    using Sentry;
    using Serilog;
    using Serilog.Configuration;
    using Serilog.Events;
    using Serilog.Exceptions;
    using Serilog.Sinks.SystemConsole.Themes;
    using TrainsOnline.Common;

    public static class SerilogConfiguration
    {
        public static void ConfigureSerilog()
        {
            LoggerSettings loggerSettigns;
            //logger configuration
            {
                IConfigurationBuilder configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory());
                configuration.AddJsonFile(GlobalAppConfig.AppSettingsFileName);

                loggerSettigns = configuration.Build().GetSection(nameof(LoggerSettings)).Get<LoggerSettings>();
            }

            LoggerConfiguration loggerConfiguration = new LoggerConfiguration()
                                         .Enrich.FromLogContext()
                                         .Enrich.WithExceptionDetails()
                                         .Enrich.WithProcessId()
                                         .Enrich.WithProcessName()
                                         .Enrich.WithThreadId();

#pragma warning disable CS0162 // Unreachable code detected
            loggerConfiguration.MinimumLevel.Information();

            if (GlobalAppConfig.DEV_MODE)
            {
                if (loggerSettigns.LogEverythingInDev)
                {
                    loggerConfiguration.MinimumLevel.Verbose();
                }
                else
                {
                    loggerConfiguration.MinimumLevel.Debug();
                }
            }
#pragma warning restore CS0162 // Unreachable code detected

            ConfigureSentry(loggerSettigns, loggerConfiguration);

            if (loggerSettigns.IsConsoleOutputEnabled)
                loggerConfiguration.WriteTo.Async(WriteToConsole(loggerSettigns));

            if (loggerSettigns.IsFileOutputEnabled)
                loggerConfiguration.WriteTo.Async(WriteToFile(loggerSettigns));

            Log.Logger = loggerConfiguration.CreateLogger();

            Log.Information($"Config file: {GlobalAppConfig.AppSettingsFileName}");
            Log.Information($"Logs are stored under: {loggerSettigns.FullPath}");
        }

        private static void ConfigureSentry(LoggerSettings loggerSettigns, LoggerConfiguration loggerConfiguration)
        {
            if (!loggerSettigns.SentryEnabled)
                return;

            loggerConfiguration.WriteTo.Sentry(o =>
            {
                // Debug and higher are stored as breadcrumbs (default is Information)
                o.MinimumBreadcrumbLevel = LogEventLevel.Debug;
                // Warning and higher is sent as event (default is Error)
                o.MinimumEventLevel = LogEventLevel.Warning;
                o.Dsn = new Dsn(loggerSettigns.SentryDSN);
                o.AttachStacktrace = true;
                o.SendDefaultPii = true;
                o.Release = GlobalAppConfig.AppInfo.SentryReleaseVersion;
                o.ReportAssemblies = true;
                o.Environment = GlobalAppConfig.DEV_MODE ? "Development" : "Production";
            });
        }

        private static Action<LoggerSinkConfiguration> WriteToConsole(LoggerSettings loggerSettigns)
        {
            return x => x.Console(outputTemplate: loggerSettigns.ConsoleOutputTemplate,
                                  theme: AnsiConsoleTheme.Literate);
        }

        private static Action<LoggerSinkConfiguration> WriteToFile(LoggerSettings loggerSettigns)
        {
            return x => x.File(loggerSettigns.FullPath,
                               outputTemplate: loggerSettigns.FileOutputTemplate,
                               fileSizeLimitBytes: loggerSettigns.FileSizeLimitInBytes,
                               rollingInterval: RollingInterval.Day,
                               rollOnFileSizeLimit: true,
                               flushToDiskInterval: TimeSpan.FromSeconds(loggerSettigns.FlushIntervalInSeconds),
                               retainedFileCountLimit: loggerSettigns.RetainedFileCountLimit,
                               shared: true);
        }
    }
}
