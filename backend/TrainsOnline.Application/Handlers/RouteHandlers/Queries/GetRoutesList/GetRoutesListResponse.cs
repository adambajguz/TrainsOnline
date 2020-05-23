namespace TrainsOnline.Application.Handlers.RouteHandlers.Queries.GetRoutesList
{
    using System;
    using System.Collections.Generic;
    using AutoMapper;
    using TrainsOnline.Application.DTO;
    using TrainsOnline.Application.Handlers.StationHandlers.Queries.GetStationsList;
    using TrainsOnline.Application.Interfaces.Mapping;
    using TrainsOnline.Domain.Entities;

    public class GetRoutesListResponse : IDataTransferObject
    {
        public IList<RouteLookupModel> Routes { get; set; } = default!;

        public class RouteLookupModel : IDataTransferObject, ICustomMapping
        {
            public Guid Id { get; set; }

            //public Guid FromId { get; set; }
            //public Guid ToId { get; set; }
            public GetStationsListResponse.StationLookupModel From { get; set; } = default!;
            public GetStationsListResponse.StationLookupModel To { get; set; } = default!;

            public DateTime DepartureTime { get; set; } = default!;
            public TimeSpan Duration { get; set; } = default!;
            public double Distance { get; set; }
            public double TicketPrice { get; set; }

            void ICustomMapping.CreateMappings(Profile configuration)
            {
                configuration.CreateMap<Route, RouteLookupModel>();
            }
        }
    }
}
