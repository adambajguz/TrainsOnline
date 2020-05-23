namespace TrainsOnline.Domain.Abstractions.Audit
{
    using System;
    using TrainsOnline.Domain.Abstractions.Enums;

    public interface IAuditLog
    {
        string TableName { get; set; }
        Guid Key { get; set; }
        AuditActions Action { get; set; }
        string? Values { get; set; }
    }
}
