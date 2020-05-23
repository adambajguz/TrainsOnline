namespace TrainsOnline.Application.Handlers.RouteLogHandlers.Commands.UpdateRouteLog
{
    using System;
    using Application.Interfaces.Mapping;
    using AutoMapper;
    using TrainsOnline.Application.DTO;
    using TrainsOnline.Domain.Entities;

    public class UpdateRouteLogRequest : IDataTransferObject, ICustomMapping
    {
        public Guid Id { get; set; }

        public Guid FromId { get; set; }
        public Guid ToId { get; set; }

        public DateTime DepartureTime { get; set; } = default!;
        public TimeSpan Duration { get; set; } = default!;
        public double Distance { get; set; }
        public double TicketPrice { get; set; }

        void ICustomMapping.CreateMappings(Profile configuration)
        {
            configuration.CreateMap<UpdateRouteLogRequest, RouteLog>();
        }
    }
}
