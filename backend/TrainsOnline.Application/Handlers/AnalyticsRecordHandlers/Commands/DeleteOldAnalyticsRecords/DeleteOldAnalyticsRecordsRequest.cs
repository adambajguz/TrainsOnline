namespace TrainsOnline.Application.Handlers.AnalyticsRecordHandlers.Commands.DeleteOldAnalyticsRecords
{
    using System;
    using TrainsOnline.Application.DTO;

    public class DeleteOldAnalyticsRecordsRequest : IDataTransferObject
    {
        public DateTime? OlderThanOrEqualToDate { get; set; }
    }
}
