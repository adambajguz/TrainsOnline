namespace TrainsOnline.Tracker.Application.DTO.Analytics
{
    using System;
    using TrainsOnline.Tracker.Application.DTO;

    public class AnalyticsRecordLookupModel : IDataTransferObject
    {
        public Guid Id { get; set; }

        public DateTime Timestamp { get; set; }
        public string Uri { get; set; }
        public ulong Visits { get; set; }
    }
}
