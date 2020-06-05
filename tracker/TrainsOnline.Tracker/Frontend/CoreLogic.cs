namespace SPA.xUnitGen.Frontend
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using CommandLine;
    using Serilog;
    using SerilogTimings;


    internal static class CoreLogic
    {

        #region Callbacks for CommandLine library
        public static void Execute(Options options)
        {
            using (Operation.Time("Created all queries. All tasks processing"))
            {
                Log.Information("Execution dir is `{Directory}`:", Directory.GetCurrentDirectory());

                string[] taskFilePathsList = new string[] { options.TaskFilePath };
                bool runAllTasks = options.RunAllTasks;


            }

            if (!options.NoPause)
                Pause();
        }

        public static void HandleOptionsErrors(IEnumerable<Error> errors)
        {
            if (errors.IsHelp() && System.Diagnostics.Debugger.IsAttached)
                Pause();
        }
        #endregion

        #region Helpers
        public static void Pause()
        {
            Console.Write($"\nPress any key to exit . . . ");

            ConsoleKey key;
            do
                key = Console.ReadKey().Key;
            while (key == ConsoleKey.LeftWindows || key == ConsoleKey.RightWindows);
        }
        #endregion
    }
}
