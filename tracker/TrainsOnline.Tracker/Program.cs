namespace TrainsOnline.Tracker
{
    using System;
    using CommandLine;
    using SPA.xUnitGen.Frontend;
    using SPA.xUnitGen.Logger;

    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Starting...");

            SerilogConfiguration.ConfigureSerilog();

            Parser.Default.ParseArguments<Options>(args)
                 .WithParsed((options) => CoreLogic.Execute(options).Wait())
                 .WithNotParsed(CoreLogic.HandleOptionsErrors);
        }
    }
}
