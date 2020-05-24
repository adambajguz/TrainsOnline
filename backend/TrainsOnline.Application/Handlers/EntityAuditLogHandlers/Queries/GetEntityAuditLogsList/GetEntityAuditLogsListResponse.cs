namespace TrainsOnline.Application.Handlers.EntityAuditLog.Queries.GetRouteLogsList
{
    using System;
    using System.Collections.Generic;
    using AutoMapper;
    using TrainsOnline.Application.DTO;
    using TrainsOnline.Application.Interfaces.Mapping;
    using TrainsOnline.Domain.Entities;

    public class GetEntityAuditLogsListResponse : IDataTransferObject
    {
        public IList<RouteLogLookupModel> EntityAuditLogs { get; set; } = default!;

        public class RouteLogLookupModel : IDataTransferObject, ICustomMapping
        {
            public Guid Id { get; set; }

            public double Distance { get; set; }
            public double TicketPrice { get; set; }

            void ICustomMapping.CreateMappings(Profile configuration)
            {
                configuration.CreateMap<RouteLog, RouteLogLookupModel>();
            }
        }
    }
}
