namespace TrainsOnline.Api
{
    using CommandLine;
    using TrainsOnline.Api.Configuration;
    using TrainsOnline.Api.RuntimeArguments;

    internal static class Program
    {
        public static void Main(string[] args)
        {
            CommandLine.Parser.Default.ParseArguments<RuntimeOptions>(args)
                                      .WithParsed(CoreLogic.Execute)
                                      .WithNotParsed(CoreLogic.HandleOptionsErrors);
        }
    }
}