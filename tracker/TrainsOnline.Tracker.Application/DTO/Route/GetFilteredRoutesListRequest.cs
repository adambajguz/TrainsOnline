namespace TrainsOnline.Tracker.Application.DTO.Route
{
    public class GetFilteredRoutesListRequest : IDataTransferObject
    {
        public string FromPattern { get; set; }
        public string ToPattern { get; set; }
        public double? MaximumTicketPrice { get; set; }
    }
}
