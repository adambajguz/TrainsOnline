namespace TrainsOnline.Application.Handlers.TicketHandlers.Queries.GetTicketDetails
{
    using System;
    using Application.Interfaces.Mapping;
    using AutoMapper;
    using Domain.Entities;
    using TrainsOnline.Application.DTO;

    public class GetTicketDetailsResponse : IDataTransferObject, ICustomMapping
    {
        public Guid Id { get; set; }

        public DateTime CreatedOn { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime LastSavedOn { get; set; }
        public Guid? LastSavedBy { get; set; }

        public Guid UserId { get; set; }
        //public Guid RouteId { get; set; }

        public TicketRouteDetailsModel Route { get; set; } = default!;

        void ICustomMapping.CreateMappings(Profile configuration)
        {
            configuration.CreateMap<Ticket, GetTicketDetailsResponse>();
        }

        public class TicketRouteDetailsModel : IDataTransferObject, ICustomMapping
        {
            public Guid Id { get; set; }

            //public Guid FromId { get; set; }
            //public Guid ToId { get; set; }

            public TicketStationDetailsModel From { get; set; } = default!;
            public TicketStationDetailsModel To { get; set; } = default!;

            public DateTime DepartureTime { get; set; } = default!;
            public TimeSpan Duration { get; set; } = default!;
            public double Distance { get; set; }
            public double TicketPrice { get; set; }

            void ICustomMapping.CreateMappings(Profile configuration)
            {
                configuration.CreateMap<Route, TicketRouteDetailsModel>();
            }

            public class TicketStationDetailsModel : IDataTransferObject, ICustomMapping
            {
                public Guid Id { get; set; }

                public string Name { get; set; } = default!;

                public double Latitude { get; set; } = default!;
                public double Longitude { get; set; } = default!;

                void ICustomMapping.CreateMappings(Profile configuration)
                {
                    configuration.CreateMap<Station, TicketStationDetailsModel>();
                }
            }
        }
    }
}
