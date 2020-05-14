namespace TrainsOnline.Application.Handlers.RouteHandlers.Queries.GetFilteredRoutesList
{
    using TrainsOnline.Application.DTO;

    public class GetFilteredRoutesListRequest : IDataTransferObject
    {
        public string? FromPattern { get; set; }
        public string? ToPattern { get; set; }
        public double? MaximumTicketPrice { get; set; }
    }
}
