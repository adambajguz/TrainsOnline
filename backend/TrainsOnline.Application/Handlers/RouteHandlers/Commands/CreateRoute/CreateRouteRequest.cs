namespace TrainsOnline.Application.Handlers.RouteHandlers.Commands.CreateRoute
{
    using System;
    using Application.Interfaces.Mapping;
    using AutoMapper;
    using TrainsOnline.Application.DTO;
    using TrainsOnline.Domain.Entities;

    public class CreateRouteRequest : IDataTransferObject, ICustomMapping
    {
        public Guid FromId { get; set; }
        public Guid ToId { get; set; }

        public DateTime DepartureTime { get; set; } = default!;
        public TimeSpan Duration { get; set; } = default!;
        public double Distance { get; set; }
        public double TicketPrice { get; set; }

        void ICustomMapping.CreateMappings(Profile configuration)
        {
            configuration.CreateMap<CreateRouteRequest, Route>();
        }
    }
}
