namespace TrainsOnline.Application.Handlers.EntityAuditLog.Queries.GetRouteLogsList
{
    using System;
    using System.Collections.Generic;
    using AutoMapper;
    using TrainsOnline.Application.DTO;
    using TrainsOnline.Application.Interfaces.Mapping;
    using TrainsOnline.Domain.Abstractions.Enums;
    using TrainsOnline.Domain.Entities;

    public class GetEntityAuditLogsForEntityListResponse : IDataTransferObject
    {

        public IList<EntityAuditLogForEntityLookupModel> EntityAuditLogs { get; set; } = default!;
        public bool Exists { get; set; }

        public GetEntityAuditLogsForEntityListResponse(IList<EntityAuditLogForEntityLookupModel> entityAuditLogs, bool exists)
        {
            EntityAuditLogs = entityAuditLogs;
            Exists = exists;
        }

        public class EntityAuditLogForEntityLookupModel : IDataTransferObject, ICustomMapping
        {
            public Guid Id { get; set; }
            public DateTime CreatedOn { get; set; }
            public Guid? CreatedBy { get; set; }

            public string TableName { get; set; } = default!;
            public AuditActions Action { get; set; }
            public string? Values { get; set; }

            void ICustomMapping.CreateMappings(Profile configuration)
            {
                configuration.CreateMap<EntityAuditLog, EntityAuditLogForEntityLookupModel>();
            }
        }
    }
}
