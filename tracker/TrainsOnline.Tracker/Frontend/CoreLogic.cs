namespace SPA.xUnitGen.Frontend
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using CommandLine;
    using Serilog;
    using SerilogTimings;
    using TrainsOnline.Desktop.Domain.RemoteDataProvider;
    using TrainsOnline.Tracker.Application.DTO;
    using TrainsOnline.Tracker.Application.DTO.Route;
    using TrainsOnline.Tracker.Application.DTO.RouteLog;
    using TrainsOnline.Tracker.Application.Exceptions;
    using TrainsOnline.Tracker.Application.Extensions;

    internal static class CoreLogic
    {

        #region Callbacks for CommandLine library
        public static async Task Execute(Options options)
        {
            using (Operation.Time("Exit"))
            {
                try
                {
                    Log.Information("Execution dir is `{Directory}`:", Directory.GetCurrentDirectory());

                    RestDataProvider dataProvider = new RestDataProvider();
                    await Run(dataProvider);
                }
                catch (RemoteDataException ex)
                {
                    Console.WriteLine(ex.GetResponse().ToPrettyJson());
                    Log.Error(ex, "RemoteDataException");
                }

            }

            if (!options.NoPause)
                Pause();
        }

        private static async Task Run(RestDataProvider dataProvider)
        {
            await Login(dataProvider);

            await GetRoutes(dataProvider);

            (Guid?, GetRouteDetailsResponse) data = await GetRoute(dataProvider);
            if (data.Item1 is null)
                return;

            Pause();

            await SimulateTrain(dataProvider, data.Item1, data.Item2);
        }

        private static async Task SimulateTrain(RestDataProvider dataProvider, Guid? routeId, GetRouteDetailsResponse routeDetails)
        {
            Random random = new Random();
            DateTime start = routeDetails.DepartureTime;
            int durationMinutes = (int)routeDetails.Duration.TotalMinutes;

            double latitudeStep = (routeDetails.From.Latitude - routeDetails.To.Latitude) / durationMinutes;
            double longitudeStep = (routeDetails.From.Longitude - routeDetails.To.Longitude) / durationMinutes;

            CreateRouteLogRequest data = new CreateRouteLogRequest
            {
                RouteId = (Guid)routeId,
                Latitude = routeDetails.From.Latitude,
                Longitude = routeDetails.From.Longitude,
                Timestamp = start
            };

            for (int i = 0; i < durationMinutes; ++i)
            {
                double speed = random.Next(0, 100);
                double voltage = random.Next(2800, 3200);
                double current = random.Next(0, 600);

                data.Latitude -= latitudeStep;
                data.Longitude -= longitudeStep;

                data.Speed = speed < 20 ? 0 : speed;
                data.Voltage = voltage;
                data.Current = speed == 0 ? 0 : current + 50;
                data.Timestamp = data.Timestamp.AddMinutes(1);

                Console.WriteLine(data.ToJson());
                await dataProvider.RouteLogCreate(data);
            }

            await dataProvider.RouteReportCreate(new IdRequest((Guid)routeId));
        }

        private static async Task<(Guid?, GetRouteDetailsResponse)> GetRoute(RestDataProvider dataProvider)
        {
            Console.Write("RouteId: ");
            string routeId = Console.ReadLine().Trim();

            if (Guid.TryParse(routeId, out Guid guid))
            {
                GetRouteDetailsResponse details = await dataProvider.GetRoute(guid);
                Console.WriteLine(details.ToPrettyJson());
                return (guid, details);
            }

            Console.WriteLine("Wrong guid");

            return (null, null);
        }

        private static async Task GetRoutes(RestDataProvider dataProvider)
        {
            GetRoutesListResponse routes = await dataProvider.GetRoutes();
            Console.WriteLine(routes.Routes.ToPrettyJson());
        }

        private static async Task Login(RestDataProvider dataProvider)
        {
            Console.WriteLine("Login");
            Console.Write("Email: ");
            string email = Console.ReadLine();

            Console.Write("Password: ");
            string password = Console.ReadLine();

            await dataProvider.Login(new TrainsOnline.Tracker.Application.DTO.Authentication.LoginRequest
            {
                Email = email.Trim(),
                Password = password.Trim()
            });
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