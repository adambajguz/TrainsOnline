namespace TrainsOnline.Tracker
{
    using CommandLine;
    using SPA.xUnitGen.Frontend;
    using SPA.xUnitGen.Logger;

    internal class Program
    {
        private static void Main(string[] args)
        {
            SerilogConfiguration.ConfigureSerilog();

            Parser.Default.ParseArguments<Options>(args)
                 .WithParsed(CoreLogic.Execute)
                 .WithNotParsed(CoreLogic.HandleOptionsErrors);
        }
    }
}
