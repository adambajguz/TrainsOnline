namespace TrainsOnline.Application.Handlers.RouteLogHandlers.Queries.GetRouteLogDetails
{
    using System;
    using Application.Interfaces.Mapping;
    using AutoMapper;
    using Domain.Entities;
    using TrainsOnline.Application.DTO;

    public class GetRouteLogDetailsResponse : IDataTransferObject, ICustomMapping
    {
        public Guid Id { get; set; }

        public DateTime CreatedOn { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime LastSavedOn { get; set; }
        public Guid? LastSavedBy { get; set; }

        public DateTime Timestamp { get; set; }
        public Guid RouteId { get; set; }

        public double Latitude { get; set; } = default!;
        public double Longitude { get; set; } = default!;

        public double Voltage { get; set; }
        public double Current { get; set; }
        public double Speed { get; set; }

        void ICustomMapping.CreateMappings(Profile configuration)
        {
            configuration.CreateMap<RouteLog, GetRouteLogDetailsResponse>();
        }
    }
}
