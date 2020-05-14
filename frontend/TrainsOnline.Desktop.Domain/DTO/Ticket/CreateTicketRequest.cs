namespace TrainsOnline.Desktop.Domain.DTO.Ticket
{
    using System;
    using TrainsOnline.Desktop.Domain.DTO;

    public class CreateTicketRequest : IDataTransferObject
    {
        public Guid UserId { get; set; }
        public Guid RouteId { get; set; }
    }
}
