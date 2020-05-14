namespace TrainsOnline.Desktop.Views.Ticket
{
    using TrainsOnline.Desktop.Domain.DTO.Ticket;

    public interface ITicketContentGridViewEvents
    {
        void OnItemSelected(UserTicketLookupModel clickedItem);
    }
}
