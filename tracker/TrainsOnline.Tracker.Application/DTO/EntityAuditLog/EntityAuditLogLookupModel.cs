namespace TrainsOnline.Tracker.Application.DTO.EntityAuditLog
{
    using System;
    using TrainsOnline.Tracker.Application.DTO;

    public class EntityAuditLogLookupModel : IDataTransferObject
    {
        public Guid Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? CreatedBy { get; set; }

        public string TableName { get; set; }
        public Guid Key { get; set; }
        public AuditActions Action { get; set; }
    }
}
