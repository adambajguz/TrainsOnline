namespace TrainsOnline.Application.Handlers.RouteLogHandlers.Queries.GetRouteLogsListByRouteId
{
    using System;
    using System.Collections.Generic;
    using AutoMapper;
    using TrainsOnline.Application.DTO;
    using TrainsOnline.Application.Interfaces.Mapping;
    using TrainsOnline.Domain.Entities;

    public class GetRouteLogsListByRouteIdResponse : IDataTransferObject
    {
        public IList<RouteLogByRouteIdLookupModel> RouteLogs { get; set; } = default!;

        public class RouteLogByRouteIdLookupModel : IDataTransferObject, ICustomMapping
        {
            public Guid Id { get; set; }

            public DateTime Timestamp { get; set; }
            public Guid RouteId { get; set; }

            public double Latitude { get; set; } = default!;
            public double Longitude { get; set; } = default!;

            public double Voltage { get; set; }
            public double Current { get; set; }
            public double Speed { get; set; }

            void ICustomMapping.CreateMappings(Profile configuration)
            {
                configuration.CreateMap<RouteLog, RouteLogLookupModel>();
            }
        }
    }
}
