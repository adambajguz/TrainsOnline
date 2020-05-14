namespace TrainsOnline.Desktop.Views.Route
{
    using static TrainsOnline.Desktop.Domain.DTO.Station.GetStationDetailsResponse;

    internal interface IStationMasterDetailDetailView
    {
        void ShowStationOnMap();
        void ShowDestinationOnMap(RouteDeparturesLookupModel route);
        void ShowRouteOnMap(RouteDeparturesLookupModel route);
        void BuyTicket(RouteDeparturesLookupModel route);
    }
}
