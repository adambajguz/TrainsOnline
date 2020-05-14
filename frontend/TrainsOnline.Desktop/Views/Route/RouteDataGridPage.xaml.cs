namespace TrainsOnline.Desktop.Views.Route
{
    using TrainsOnline.Desktop.ViewModels.Route;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Navigation;

    public sealed partial class RouteDataGridPage : Page, IRouteDataGridView
    {
        // TODO WTS: Change the grid as appropriate to your app, adjust the column definitions on ExampleDataGridPage.xaml.
        // For more details see the documentation at https://docs.microsoft.com/windows/communitytoolkit/controls/datagrid
        public RouteDataGridPage()
        {
            InitializeComponent();
        }

        private RouteDataGridViewModel ViewModel => DataContext as RouteDataGridViewModel;

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            await ViewModel.LoadDataAsync();
        }
    }
}
