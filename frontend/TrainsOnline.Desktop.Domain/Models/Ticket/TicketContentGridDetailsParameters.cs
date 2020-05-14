namespace TrainsOnline.Desktop.Domain.Models.Ticket
{
    using System;

    public readonly struct TicketContentGridDetailsParameters
    {
        public Guid TicketId { get; }

        public TicketContentGridDetailsParameters(Guid ticketId)
        {
            TicketId = ticketId;
        }
    }
}
