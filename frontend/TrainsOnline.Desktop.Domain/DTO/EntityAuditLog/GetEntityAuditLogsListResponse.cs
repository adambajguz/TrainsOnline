namespace TrainsOnline.Desktop.Domain.DTO.EntityAuditLog
{
    using System.Collections.Generic;

    public class GetEntityAuditLogsListResponse : IDataTransferObject
    {
        public IList<EntityAuditLogLookupModel> EntityAuditLogs { get; set; }
    }
}
