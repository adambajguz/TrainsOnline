namespace TrainsOnline.Desktop.Domain.DTO.Analytics
{
    using System.Collections.Generic;

    public class GetAnalyticsRecordsListResponse : IDataTransferObject
    {
        public IList<AnalyticsRecordLookupModel> AnalyticsRecords { get; set; }
    }
}
