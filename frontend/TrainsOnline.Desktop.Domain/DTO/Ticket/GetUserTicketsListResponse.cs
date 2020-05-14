namespace TrainsOnline.Desktop.Domain.DTO.Ticket
{
    using System;
    using System.Collections.Generic;
    using TrainsOnline.Desktop.Domain.DTO;

    public class GetUserTicketsListResponse : IDataTransferObject
    {
        public List<UserTicketLookupModel> Tickets { get; set; }
    }

    public class UserTicketLookupModel : IDataTransferObject
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }
        //public Guid RouteId { get; set; }

        public UserTicketRouteLookupModel Route { get; set; }

        public class UserTicketRouteLookupModel : IDataTransferObject
        {
            public Guid Id { get; set; }

            //public Guid FromId { get; set; }
            //public Guid ToId { get; set; }

            public UserTicketRouteStationLookupModel From { get; set; }
            public UserTicketRouteStationLookupModel To { get; set; }

            public DateTime DepartureTime { get; set; }
            public TimeSpan Duration { get; set; }
            public double Distance { get; set; }
            public double TicketPrice { get; set; }

            public class UserTicketRouteStationLookupModel : IDataTransferObject
            {
                public Guid Id { get; set; }

                public string Name { get; set; }

                public double Latitude { get; set; }
                public double Longitude { get; set; }
            }
        }
    }
}
