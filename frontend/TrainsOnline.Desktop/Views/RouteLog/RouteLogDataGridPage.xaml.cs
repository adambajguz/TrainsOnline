namespace TrainsOnline.Desktop.Views.RouteLog
{
    using TrainsOnline.Desktop.ViewModels.RouteLog;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Navigation;

    public sealed partial class RouteLogDataGridPage : Page, IRouteLogDataGridView
    {
        // TODO WTS: Change the grid as appropriate to your app, adjust the column definitions on ExampleDataGridPage.xaml.
        // For more details see the documentation at https://docs.microsoft.com/windows/communitytoolkit/controls/datagrid
        public RouteLogDataGridPage()
        {
            InitializeComponent();
        }

        private RouteLogDataGridViewModel ViewModel => DataContext as RouteLogDataGridViewModel;

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            await ViewModel.LoadDataAsync();
        }
    }
}
