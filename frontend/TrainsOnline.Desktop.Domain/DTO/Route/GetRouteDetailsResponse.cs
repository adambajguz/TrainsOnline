namespace TrainsOnline.Desktop.Domain.DTO.Route
{
    using System;
    using TrainsOnline.Desktop.Common.GeoHelpers;
    using TrainsOnline.Desktop.Domain.DTO;

    public class GetRouteDetailsResponse : IDataTransferObject
    {
        public Guid Id { get; set; }

        public DateTime CreatedOn { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime LastSavedOn { get; set; }
        public Guid? LastSavedBy { get; set; }

        //public Guid FromId { get; set; }
        //public Guid ToId { get; set; }

        public RouteStationLookupModel From { get; set; }
        public RouteStationLookupModel To { get; set; }

        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime => DepartureTime.Add(Duration);
        public TimeSpan Duration { get; set; }
        public double Distance { get; set; }
        public double TicketPrice { get; set; }

        public class RouteStationLookupModel : IDataTransferObject
        {
            public Guid Id { get; set; }

            public string Name { get; set; }

            public double Latitude { get; set; }
            public double Longitude { get; set; }

            public GeoCoordinate Coordinates => GeoCoordinate.FromDouble(Latitude, Longitude);
        }
    }
}
