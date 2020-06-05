namespace TrainsOnline.Tracker.Application.DTO.EntityAuditLog
{
    using System.Collections.Generic;
    using TrainsOnline.Tracker.Application.DTO;

    public class GetEntityAuditLogsListResponse : IDataTransferObject
    {
        public IList<EntityAuditLogLookupModel> EntityAuditLogs { get; set; }
    }
}
