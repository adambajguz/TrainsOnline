namespace TrainsOnline.Application.Handlers.RouteLogHandlers.Commands.CreateRouteLog
{
    using System;
    using Application.Interfaces.Mapping;
    using AutoMapper;
    using TrainsOnline.Application.DTO;
    using TrainsOnline.Domain.Entities;

    public class CreateRouteLogRequest : IDataTransferObject, ICustomMapping
    {
        public DateTime Timestamp { get; set; }
        public Guid RouteId { get; set; }

        public double Latitude { get; set; } = default!;
        public double Longitude { get; set; } = default!;

        public double Voltage { get; set; }
        public double Current { get; set; }
        public double Speed { get; set; }

        void ICustomMapping.CreateMappings(Profile configuration)
        {
            configuration.CreateMap<CreateRouteLogRequest, RouteLog>();
        }
    }
}
