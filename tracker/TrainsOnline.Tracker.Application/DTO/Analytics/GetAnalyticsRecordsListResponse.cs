namespace TrainsOnline.Tracker.Application.DTO.Analytics
{
    using System.Collections.Generic;
    using TrainsOnline.Tracker.Application.DTO;

    public class GetAnalyticsRecordsListResponse : IDataTransferObject
    {
        public IList<AnalyticsRecordLookupModel> AnalyticsRecords { get; set; }
    }
}
