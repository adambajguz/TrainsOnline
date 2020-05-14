namespace TrainsOnline.Desktop.Domain.DTO.Ticket
{
    using System;
    using TrainsOnline.Desktop.Domain.DTO;

    public class UpdateTicketRequest : IDataTransferObject
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }
        public Guid RouteId { get; set; }
    }
}
