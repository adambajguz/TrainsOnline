namespace TrainsOnline.Application.Handlers.AnalyticsRecordHandlers.Queries.GetRouteReportsList
{
    using System;
    using System.Collections.Generic;
    using AutoMapper;
    using TrainsOnline.Application.DTO;
    using TrainsOnline.Application.Interfaces.Mapping;
    using TrainsOnline.Domain.Entities;

    public class GetAnalyticsRecordsListResponse : IDataTransferObject
    {
        public IList<AnalyticsRecordLookupModel> AnalyticsRecords { get; set; } = default!;

        public class AnalyticsRecordLookupModel : IDataTransferObject, ICustomMapping
        {
            public Guid Id { get; set; }

            public DateTime Timestamp { get; set; }
            public string Uri { get; set; } = default!;
            public ulong Visits { get; set; }

            void ICustomMapping.CreateMappings(Profile configuration)
            {
                configuration.CreateMap<AnalyticsRecord, AnalyticsRecordLookupModel>();
            }
        }
    }
}
