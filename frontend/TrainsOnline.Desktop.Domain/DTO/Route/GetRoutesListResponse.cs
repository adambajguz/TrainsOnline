namespace TrainsOnline.Desktop.Domain.DTO.Route
{
    using System;
    using System.Collections.Generic;
    using TrainsOnline.Desktop.Domain.DTO;
    using static TrainsOnline.Desktop.Domain.DTO.Station.GetStationsListResponse;

    public class GetRoutesListResponse : IDataTransferObject
    {
        public List<RouteLookupModel> Routes { get; set; }

        public class RouteLookupModel : IDataTransferObject
        {
            public Guid Id { get; set; }

            //public Guid FromId { get; set; }
            //public Guid ToId { get; set; }
            public StationLookupModel From { get; set; }
            public StationLookupModel To { get; set; }

            public DateTime DepartureTime { get; set; }
            public TimeSpan Duration { get; set; }
            public double Distance { get; set; }
            public double TicketPrice { get; set; }
        }
    }
}
