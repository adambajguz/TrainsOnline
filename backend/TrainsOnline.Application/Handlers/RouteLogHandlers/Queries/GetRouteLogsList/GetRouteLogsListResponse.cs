﻿namespace TrainsOnline.Application.Handlers.RouteLogHandlers.Queries.GetRouteLogsList
{
    using System;
    using System.Collections.Generic;
    using AutoMapper;
    using TrainsOnline.Application.DTO;
    using TrainsOnline.Application.Interfaces.Mapping;
    using TrainsOnline.Domain.Entities;

    public class GetRouteLogsListResponse : IDataTransferObject
    {
        public List<RouteLookupModel> RouteLogs { get; set; } = default!;

        public class RouteLookupModel : IDataTransferObject, ICustomMapping
        {
            public Guid Id { get; set; }

            public double Distance { get; set; }
            public double TicketPrice { get; set; }

            void ICustomMapping.CreateMappings(Profile configuration)
            {
                configuration.CreateMap<RouteLog, RouteLookupModel>();
            }
        }
    }
}