namespace TrainsOnline.Desktop.ViewModels.General
{
    using Caliburn.Micro;
    using TrainsOnline.Desktop.Domain.Models.General;
    using TrainsOnline.Desktop.Views.General;
    using Windows.Devices.Geolocation;

    public class GeneralMapViewModel : Screen, IGeneralMapViewEvents
    {
        // TODO WTS: Set your preferred default zoom level
        private const double DefaultZoomLevel = 17;
        private const double DefaultZoomStep = 0.1;

        // TODO WTS: Set your preferred default location if a geolock can't be found.
        private readonly BasicGeoposition _defaultPosition = new BasicGeoposition()
        {
            Latitude = 47.609425,
            Longitude = -122.3417
        };

        private double _zoomLevel;

        public double ZoomLevel
        {
            get => _zoomLevel;
            set => Set(ref _zoomLevel, value);
        }

        private Geopoint _center;

        public Geopoint Center
        {
            get => _center;
            set => Set(ref _center, value);
        }

        private string _mapServiceToken;

        public string MapServiceToken
        {
            get => _mapServiceToken;
            set => Set(ref _mapServiceToken, value);
        }

        public GeneralMapViewParameters Parameter { get; set; }

        public GeneralMapViewModel()
        {
            Center = new Geopoint(_defaultPosition);
            ZoomLevel = DefaultZoomLevel;
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            SetCenter();

            // TODO WTS: Set your map service token. If you don't have one, request from https://www.bingmapsportal.com/
            MapServiceToken = "ioTqhTtWjH1AHQSTdqM3~cwrvRdJBaUYH_uufKdr50g~Au3dHttbUXjJxBL8I9fV6ItiDau37iX6PKfggfU9nq2VgR8IsSoOVYICu88E93in"; //TODO: add to some sort of config
            IGeneralMapView view = GetView() as IGeneralMapView;

            view?.AddMapIcon(Center, "#1");

            for (int i = 1; i < Parameter.GeoCoordinates.Length; ++i)
            {
                view?.AddMapIcon(new Geopoint(new BasicGeoposition
                {
                    Latitude = Parameter.GeoCoordinates[i].RawLatitude,
                    Longitude = Parameter.GeoCoordinates[i].RawLongitude
                }), $"#{i + 1}");
            }
        }

        private void SetCenter()
        {
            if (Parameter.GeoCoordinates.Length > 0)
            {
                Center = new Geopoint(new BasicGeoposition
                {
                    Latitude = Parameter.GeoCoordinates[0].RawLatitude,
                    Longitude = Parameter.GeoCoordinates[0].RawLongitude
                });
            }
        }

        public void ResetView()
        {
            SetCenter();
        }

        public void ZoomOut()
        {
            ZoomLevel -= DefaultZoomStep;
        }

        public void ResetZoom()
        {
            ZoomLevel = DefaultZoomLevel;
        }

        public void ZoomIn()
        {
            ZoomLevel += DefaultZoomStep;
        }
    }
}
