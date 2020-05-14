namespace TrainsOnline.Desktop.Domain.Models.General
{
    using TrainsOnline.Desktop.Common.GeoHelpers;

    public readonly struct GeneralMapViewParameters
    {
        public GeoCoordinate[] GeoCoordinates { get; }
        public string LocationName { get; }

        public GeneralMapViewParameters(GeoCoordinate[] geoCoordinates, string locationName = "")
        {
            GeoCoordinates = geoCoordinates;
            LocationName = locationName;
        }
    }
}
