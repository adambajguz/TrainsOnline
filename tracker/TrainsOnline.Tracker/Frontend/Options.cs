namespace SPA.xUnitGen.Frontend
{
    using CommandLine;

    internal class Options
    {
        [Value(0, MetaName = "TaskFilePath", HelpText = "Path to task file")]
        public string TaskFilePath { get; set; }

        [Option('p', "no-pause",
        Default = false,
        HelpText = "No pause before exit")]
        public bool NoPause { get; set; }

        [Option('a', "run-all",
        Default = false,
        HelpText = "Run all tasks")]
        public bool RunAllTasks { get; set; }
    }
}
