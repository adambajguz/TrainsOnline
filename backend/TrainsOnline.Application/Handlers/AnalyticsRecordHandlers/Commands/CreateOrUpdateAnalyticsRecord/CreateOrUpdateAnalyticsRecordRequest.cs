namespace TrainsOnline.Application.Handlers.AnalyticsRecordHandlers.Commands.CreateOrUpdateAnalyticsRecord
{
    using System;
    using Application.Interfaces.Mapping;
    using AutoMapper;
    using TrainsOnline.Application.DTO;
    using TrainsOnline.Domain.Entities;

    public class CreateOrUpdateAnalyticsRecordRequest : IDataTransferObject, ICustomMapping
    {
        public string? Uri { get; set; }
        public string? UserAgent { get; set; }
        public string? Ip { get; set; }

        void ICustomMapping.CreateMappings(Profile configuration)
        {
            configuration.CreateMap<CreateOrUpdateAnalyticsRecordRequest, AnalyticsRecord>();
        }
    }
}
