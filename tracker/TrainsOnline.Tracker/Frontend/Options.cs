namespace SPA.xUnitGen.Frontend
{
    using CommandLine;

    internal class Options
    {
        [Option('p', "no-pause",
        Default = false,
        HelpText = "No pause before exit")]
        public bool NoPause { get; set; }
    }
}
