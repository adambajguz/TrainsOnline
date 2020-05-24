namespace TrainsOnline.Application.Handlers.EntityAuditLog.Queries.GetRouteLogsList
{
    using System;
    using System.Collections.Generic;
    using AutoMapper;
    using TrainsOnline.Application.DTO;
    using TrainsOnline.Application.Interfaces.Mapping;
    using TrainsOnline.Domain.Abstractions.Enums;
    using TrainsOnline.Domain.Entities;

    public class GetEntityAuditLogsListResponse : IDataTransferObject
    {
        public IList<EntityAuditLogLookupModel> EntityAuditLogs { get; set; } = default!;

        public class EntityAuditLogLookupModel : IDataTransferObject, ICustomMapping
        {
            public Guid Id { get; set; }
            public DateTime CreatedOn { get; set; }
            public Guid? CreatedBy { get; set; }

            public string TableName { get; set; } = default!;
            public Guid Key { get; set; } = default!;
            public AuditActions Action { get; set; }
            public string? Values { get; set; }

            void ICustomMapping.CreateMappings(Profile configuration)
            {
                configuration.CreateMap<EntityAuditLog, EntityAuditLogLookupModel>();
            }
        }
    }
}
