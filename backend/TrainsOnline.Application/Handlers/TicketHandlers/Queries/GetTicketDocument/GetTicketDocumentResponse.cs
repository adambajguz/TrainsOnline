namespace TrainsOnline.Application.Handlers.TicketHandlers.Queries.GetTicketDocument
{
    using System;
    using Application.Interfaces.Mapping;
    using AutoMapper;
    using Domain.Entities;
    using TrainsOnline.Application.DTO;

    [Serializable]
    public class GetTicketDocumentResponse : IDataTransferObject, ICustomMapping
    {
        public Guid Id { get; set; }

        public byte[] Document { get; set; } = default!;

        void ICustomMapping.CreateMappings(Profile configuration)
        {
            configuration.CreateMap<Ticket, GetTicketDocumentResponse>();
        }
    }
}
