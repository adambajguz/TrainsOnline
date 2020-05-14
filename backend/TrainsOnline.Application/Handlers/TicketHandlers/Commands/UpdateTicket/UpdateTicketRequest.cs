namespace TrainsOnline.Application.Handlers.TicketHandlers.Commands.UpdateTicket
{
    using System;
    using Application.Interfaces.Mapping;
    using AutoMapper;
    using TrainsOnline.Application.DTO;
    using TrainsOnline.Domain.Entities;

    public class UpdateTicketRequest : IDataTransferObject, ICustomMapping
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }
        public Guid RouteId { get; set; }

        void ICustomMapping.CreateMappings(Profile configuration)
        {
            configuration.CreateMap<UpdateTicketRequest, Ticket>();
        }
    }
}
