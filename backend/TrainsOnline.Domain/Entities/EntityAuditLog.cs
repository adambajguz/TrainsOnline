namespace TrainsOnline.Domain.Entities
{
    using System;
    using TrainsOnline.Domain.Abstractions.Audit;
    using TrainsOnline.Domain.Abstractions.Base;
    using TrainsOnline.Domain.Abstractions.Enums;

    public class EntityAuditLog : IBaseRelationalEntity, IEntityCreation, IAuditLog
    {
        public Guid Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? CreatedBy { get; set; }

        public string TableName { get; set; } = default!;
        public Guid Key { get; set; } = default!;
        public AuditActions Action { get; set; }
        public string? Values { get; set; }
    }
}
