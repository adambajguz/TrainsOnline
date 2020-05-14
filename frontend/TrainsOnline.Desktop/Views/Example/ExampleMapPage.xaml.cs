namespace TrainsOnline.Desktop.Views.Example
{
    using System;
    using TrainsOnline.Desktop.ViewModels.Example;
    using Windows.Devices.Geolocation;
    using Windows.Foundation;
    using Windows.Storage.Streams;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Controls.Maps;

    public sealed partial class ExampleMapPage : Page, IExampleMapView
    {
        public ExampleMapPage()
        {
            InitializeComponent();
        }

        private ExampleMapViewModel ViewModel => DataContext as ExampleMapViewModel;

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

            mapControl.MapElements.Add(mapIcon);
        }
    }
}
