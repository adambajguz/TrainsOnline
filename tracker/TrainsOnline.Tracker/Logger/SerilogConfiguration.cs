namespace SPA.xUnitGen.Logger
{
    using System;
    using Serilog;
    using Serilog.Events;
    using Serilog.Exceptions;
    using Serilog.Sinks.SystemConsole.Themes;

    public static class SerilogConfiguration
    {
        public static void ConfigureSerilog()
        {
            LoggerSettings loggerSettigns = LoggerSettings.Default;

            LoggerConfiguration loggerConfiguration = new LoggerConfiguration()
                                         .Enrich.WithExceptionDetails()
                                         .Enrich.WithProcessId()
                                         .Enrich.WithProcessName()
                                         .Enrich.WithThreadId();

            loggerConfiguration.MinimumLevel.Override("Microsoft", LogEventLevel.Debug);
            loggerConfiguration.MinimumLevel.Verbose();
            //loggerConfiguration.WriteTo.Async(a => a.Logger(WriteToConsole(loggerSettigns)));

            Log.Logger = loggerConfiguration.WriteTo.Async(WriteToFile(loggerSettigns))
                                            .CreateLogger();

            Log.Information($"Logs are stored under: {loggerSettigns.FullPath}");
        }

        private static Action<LoggerConfiguration> WriteToConsole(LoggerSettings loggerSettigns)
        {
            return b => b.WriteTo.Async(c => c.Console(outputTemplate: loggerSettigns.ConsoleOutputTemplate,
                                                       restrictedToMinimumLevel: LogEventLevel.Information,
                                                       theme: AnsiConsoleTheme.None));
        }

        private static Action<Serilog.Configuration.LoggerSinkConfiguration> WriteToFile(LoggerSettings loggerSettigns)
        {
            return a => a.File(loggerSettigns.FullPath,
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
