namespace TrainsOnline.Api.Configuration
{
    using CommandLine;

    internal class RuntimeOptions
    {
        [Option('r', "run",
                Default = false,
                SetName = "runtime",
                HelpText = "Run webhost")]
        public bool Run { get; set; }

        [Option('m', "ef-migrate",
                Default = false,
                SetName = "runtime",
                HelpText = "Apply entity framework migrations and exit")]
        public bool EfMigrate { get; set; }

        [Option('c', "ef-migrate-check",
                Default = false,
                SetName = "runtime",
                HelpText = "Check the status of entity framework migrations")]
        public bool EfMigrateCheck { get; set; }
    }
}
