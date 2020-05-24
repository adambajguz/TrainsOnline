namespace TrainsOnline.Application.Handlers.EntityAuditLog.Queries.GetRouteLogDetails
{
    using System;
    using Application.Interfaces.Mapping;
    using AutoMapper;
    using Domain.Entities;
    using TrainsOnline.Application.DTO;
    using TrainsOnline.Domain.Abstractions.Enums;

    public class GetEntityAuditLogDetailsResponse : IDataTransferObject, ICustomMapping
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
            configuration.CreateMap<EntityAuditLog, GetEntityAuditLogDetailsResponse>();
        }
    }
}
