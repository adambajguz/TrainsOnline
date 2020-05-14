namespace TrainsOnline.Desktop.ViewModels.Example
{
    using Caliburn.Micro;
    using TrainsOnline.Desktop.Helpers;
    using TrainsOnline.Desktop.Services;
    using TrainsOnline.Desktop.Views.Example;
    using Windows.Devices.Geolocation;

    public class ExampleMapViewModel : Screen
    {
        // TODO WTS: Set your preferred default zoom level
        private const double DefaultZoomLevel = 17;

        private readonly LocationService _locationService;

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

        public ExampleMapViewModel()
        {
            _locationService = new LocationService();
            Center = new Geopoint(_defaultPosition);
            ZoomLevel = DefaultZoomLevel;
        }

        protected override async void OnInitialize()
        {
            base.OnInitialize();

            if (_locationService != null)
            {
                _locationService.PositionChanged += LocationServicePositionChanged;

                bool initializationSuccessful = await _locationService.InitializeAsync();

                if (initializationSuccessful)
                {
                    await _locationService.StartListeningAsync();
                }

                if (initializationSuccessful && _locationService.CurrentPosition != null)
                {
                    Center = _locationService.CurrentPosition.Coordinate.Point;
                }
                else
                {
                    Center = new Geopoint(_defaultPosition);
                }
            }

            // TODO WTS: Set your map service token. If you don't have one, request from https://www.bingmapsportal.com/
            MapServiceToken = "ioTqhTtWjH1AHQSTdqM3~cwrvRdJBaUYH_uufKdr50g~Au3dHttbUXjJxBL8I9fV6ItiDau37iX6PKfggfU9nq2VgR8IsSoOVYICu88E93in"; //TODO: add to some sort of config
            IExampleMapView view = GetView() as IExampleMapView;

            view?.AddMapIcon(Center, "Map_YourLocation".GetLocalized());
        }

        protected override void OnDeactivate(bool close)
        {
            base.OnDeactivate(close);

            if (_locationService != null)
            {
                _locationService.PositionChanged -= LocationServicePositionChanged;
                _locationService.StopListening();
            }
        }

        private void LocationServicePositionChanged(object sender, Geoposition geoposition)
        {
            if (geoposition != null)
            {
                Center = geoposition.Coordinate.Point;
            }
        }
    }
}
