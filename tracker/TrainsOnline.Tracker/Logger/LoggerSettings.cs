namespace SPA.xUnitGen.Logger
{
    using System.IO;

    public sealed class LoggerSettings
    {
        public string FilePath { get; set; }
        public string FileNameTemplate { get; set; }
        public string FullPath => Directory.GetCurrentDirectory() + FilePath + FileNameTemplate;

        public int RetainedFileCountLimit { get; set; }
        public int FileSizeLimitInBytes { get; set; }
        public int FlushIntervalInSeconds { get; set; }

        public string FileOutputTemplate { get; set; }

        public string ConsoleOutputTemplate { get; set; }

        public bool LogEverythingInDev { get; set; }

        public static LoggerSettings Default => new LoggerSettings()
        {
            FilePath = "\\logs\\",
            FileNameTemplate = "tracker_.log",
            RetainedFileCountLimit = 10,
            FileSizeLimitInBytes = 10485760,
            FlushIntervalInSeconds = 1,
            FileOutputTemplate = "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] <{ProcessName} {ProcessId}:{ThreadId}> {Message:lj}{NewLine}{Exception}",
            ConsoleOutputTemplate = "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
            LogEverythingInDev = true,
        };
    }
}
