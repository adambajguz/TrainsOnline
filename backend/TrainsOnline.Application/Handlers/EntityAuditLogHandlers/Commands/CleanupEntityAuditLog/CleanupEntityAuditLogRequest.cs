namespace TrainsOnline.Application.Handlers.EntityAuditLogHandlers.Commands.CleanupEntityAuditLog
{
    using System;
    using TrainsOnline.Application.DTO;

    public class CleanupEntityAuditLogRequest : IDataTransferObject
    {
        public DateTime CreatedOn { get; set; }
        public string? TableName { get; set; }
        public Guid? Key { get; set; }
    }
}
