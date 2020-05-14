namespace TrainsOnline.Desktop.Domain.DTO.Ticket
{
    using System;
    using TrainsOnline.Desktop.Domain.DTO;

    public class GetTicketDocumentResponse : IDataTransferObject
    {
        public Guid Id { get; set; }

        public string Document { get; set; }
    }
}
