namespace TrainsOnline.Desktop.Views.Example
{
    using Windows.Devices.Geolocation;

    public interface IExampleMapView
    {
        void AddMapIcon(Geopoint position, string title);
    }
}
