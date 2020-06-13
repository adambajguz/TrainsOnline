namespace TrainsOnline.Application.Handlers.EntityAuditLog.Commands.CreateRouteLog
{
    using System;
    using TrainsOnline.Application.DTO;

    public class RevertUsingEntityAuditLogRequest : IDataTransferObject
    {
        public Guid EntityId { get; set; }
        public Guid ToId { get; set; }
    }
}
