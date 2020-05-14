namespace TrainsOnline.Desktop.Views.General
{
    using System;
    using TrainsOnline.Desktop.ViewModels.General;
    using Windows.Devices.Geolocation;
    using Windows.Foundation;
    using Windows.Storage.Streams;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Controls.Maps;

    public sealed partial class GeneralMapPage : Page, IGeneralMapView
    {
        public GeneralMapPage()
        {
            InitializeComponent();
        }

        private GeneralMapViewModel ViewModel => DataContext as GeneralMapViewModel;

        public void AddMapIcon(Geopoint position, string title)
        {
            MapIcon mapIcon = new MapIcon()
            {
                Location = position,
                NormalizedAnchorPoint = new Point(0.5, 1.0),
                Title = title,
                Image = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/map.png")),
                ZIndex = 0
            };

            Map.MapElements.Add(mapIcon);
        }
    }
}
