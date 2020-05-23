namespace TrainsOnline.Application.Handlers.StationHandlers.Queries.GetStationDetails
{
    using System;
    using System.Collections.Generic;
    using Application.Interfaces.Mapping;
    using AutoMapper;
    using Domain.Entities;
    using TrainsOnline.Application.DTO;

    public class GetStationDetailsResponse : IDataTransferObject, ICustomMapping
    {
        public Guid Id { get; set; }

        public DateTime CreatedOn { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime LastSavedOn { get; set; }
        public Guid? LastSavedBy { get; set; }

        public string Name { get; set; } = default!;

        public double Latitude { get; set; } = default!;
        public double Longitude { get; set; } = default!;

        public IList<RouteDeparturesLookupModel> Departures { get; set; } = default!;

        void ICustomMapping.CreateMappings(Profile configuration)
        {
            configuration.CreateMap<Station, GetStationDetailsResponse>();
        }

        public class RouteDeparturesLookupModel : IDataTransferObject, ICustomMapping
        {
            public Guid Id { get; set; }

            //public Guid ToId { get; set; }
            public RouteDeparturesToLookupModel To { get; set; } = default!;

            public DateTime DepartureTime { get; set; } = default!;
            public TimeSpan Duration { get; set; } = default!;
            public double Distance { get; set; }
            public double TicketPrice { get; set; }

            void ICustomMapping.CreateMappings(Profile configuration)
            {
                configuration.CreateMap<Route, RouteDeparturesLookupModel>();
            }

            public class RouteDeparturesToLookupModel : IDataTransferObject, ICustomMapping
            {
                public Guid Id { get; set; }

                public string Name { get; set; } = default!;

                public double Latitude { get; set; } = default!;
                public double Longitude { get; set; } = default!;

                void ICustomMapping.CreateMappings(Profile configuration)
                {
                    configuration.CreateMap<Station, RouteDeparturesToLookupModel>();
                }
            }
        }
    }
}
