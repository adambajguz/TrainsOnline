namespace TrainsOnline.Desktop.Views.Example
{
    using TrainsOnline.Desktop.ViewModels.Example;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Navigation;

    public sealed partial class ExampleContentGridDetailPage : Page
    {
        public ExampleContentGridDetailPage()
        {
            InitializeComponent();
        }

        private ExampleContentGridDetailViewModel ViewModel => DataContext as ExampleContentGridDetailViewModel;

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is long orderID)
            {
                await ViewModel.InitializeAsync(orderID);
            }
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
            if (e.NavigationMode == NavigationMode.Back)
            {
                ViewModel.SetListDataItemForNextConnectedAnimation();
            }
        }
    }
}
