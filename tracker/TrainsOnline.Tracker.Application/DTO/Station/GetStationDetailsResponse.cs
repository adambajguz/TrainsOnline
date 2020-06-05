namespace TrainsOnline.Tracker.Application.DTO.Station
{
    using System;
    using System.Collections.Generic;
    using TrainsOnline.Desktop.Common.GeoHelpers;

    public class GetStationDetailsResponse : IDataTransferObject
    {
        public Guid Id { get; set; }

        public DateTime CreatedOn { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime LastSavedOn { get; set; }
        public Guid? LastSavedBy { get; set; }

        public string Name { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public GeoCoordinate Coordinates => GeoCoordinate.FromDouble(Latitude, Longitude);

        public List<RouteDeparturesLookupModel> Departures { get; set; }

        public class RouteDeparturesLookupModel : IDataTransferObject
        {
            public Guid Id { get; set; }

            //public Guid ToId { get; set; }
            public RouteDeparturesToLookupModel To { get; set; }

            public DateTime DepartureTime { get; set; }
            public TimeSpan Duration { get; set; }
            public double Distance { get; set; }
            public double TicketPrice { get; set; }

            public class RouteDeparturesToLookupModel : IDataTransferObject
            {
                public Guid Id { get; set; }

                public string Name { get; set; }

                public double Latitude { get; set; }
                public double Longitude { get; set; }

                public GeoCoordinate Coordinates => GeoCoordinate.FromDouble(Latitude, Longitude);
            }
        }
    }
}
