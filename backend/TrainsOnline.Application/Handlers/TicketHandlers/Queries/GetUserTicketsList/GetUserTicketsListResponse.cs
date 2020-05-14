namespace TrainsOnline.Application.Handlers.TicketHandlers.Queries.GetTicketsList
{
    using System;
    using System.Collections.Generic;
    using AutoMapper;
    using TrainsOnline.Application.DTO;
    using TrainsOnline.Application.Interfaces.Mapping;
    using TrainsOnline.Domain.Entities;

    public class GetUserTicketsListResponse : IDataTransferObject
    {
        public List<UserTicketLookupModel> Tickets { get; set; } = default!;

        public class UserTicketLookupModel : IDataTransferObject, ICustomMapping
        {
            public Guid Id { get; set; }

            public Guid UserId { get; set; }
            //public Guid RouteId { get; set; }

            public UserTicketRouteLookupModel Route { get; set; } = default!;

            void ICustomMapping.CreateMappings(Profile configuration)
            {
                configuration.CreateMap<Ticket, UserTicketLookupModel>();
            }

            public class UserTicketRouteLookupModel : IDataTransferObject, ICustomMapping
            {
                public Guid Id { get; set; }

                //public Guid FromId { get; set; }
                //public Guid ToId { get; set; }

                public UserTicketRouteStationLookupModel From { get; set; } = default!;
                public UserTicketRouteStationLookupModel To { get; set; } = default!;

                public DateTime DepartureTime { get; set; } = default!;
                public TimeSpan Duration { get; set; } = default!;
                public double Distance { get; set; }
                public double TicketPrice { get; set; }

                void ICustomMapping.CreateMappings(Profile configuration)
                {
                    configuration.CreateMap<Route, UserTicketRouteLookupModel>();
                }

                public class UserTicketRouteStationLookupModel : IDataTransferObject, ICustomMapping
                {
                    public Guid Id { get; set; }

                    public string Name { get; set; } = default!;

                    public double Latitude { get; set; } = default!;
                    public double Longitude { get; set; } = default!;

                    void ICustomMapping.CreateMappings(Profile configuration)
                    {
                        configuration.CreateMap<Station, UserTicketRouteStationLookupModel>();
                    }
                }
            }
        }
    }
}
