namespace TrainsOnline.Desktop.Views.General
{
    using Windows.Devices.Geolocation;

    public interface IGeneralMapView
    {
        void AddMapIcon(Geopoint position, string title);
    }
}
