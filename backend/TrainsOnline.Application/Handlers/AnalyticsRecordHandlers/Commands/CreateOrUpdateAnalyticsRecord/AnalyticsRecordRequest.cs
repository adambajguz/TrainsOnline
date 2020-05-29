namespace TrainsOnline.Application.Handlers.AnalyticsRecordHandlers.Commands.CreateRouteReport
{
    using System;
    using Application.Interfaces.Mapping;
    using AutoMapper;
    using TrainsOnline.Application.DTO;
    using TrainsOnline.Domain.Entities;

    public class AnalyticsRecordRequest : IDataTransferObject, ICustomMapping
    {
        public string Uri { get; set; } = default!;

        void ICustomMapping.CreateMappings(Profile configuration)
        {
            configuration.CreateMap<AnalyticsRecordRequest, AnalyticsRecord>()
                         .ForMember(dest => dest.Timestamp, opt => opt.MapFrom(src => DateTime.UtcNow.Date));
        }
    }
}
