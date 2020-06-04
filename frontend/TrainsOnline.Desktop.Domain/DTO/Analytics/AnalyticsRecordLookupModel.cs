namespace TrainsOnline.Desktop.Domain.DTO.Analytics
{
    using System;

    public class AnalyticsRecordLookupModel : IDataTransferObject
    {
        public Guid Id { get; set; }

        public DateTime Timestamp { get; set; }
        public string Uri { get; set; }
        public ulong Visits { get; set; }
    }
}
