namespace TrainsOnline.Desktop.Views.RouteReport
{
    using TrainsOnline.Desktop.ViewModels.RouteReport;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Navigation;

    public sealed partial class RouteReportDataGridPage : Page, IRouteReportDataGridView
    {
        // TODO WTS: Change the grid as appropriate to your app, adjust the column definitions on ExampleDataGridPage.xaml.
        // For more details see the documentation at https://docs.microsoft.com/windows/communitytoolkit/controls/datagrid
        public RouteReportDataGridPage()
        {
            InitializeComponent();
        }

        private RouteReportDataGridViewModel ViewModel => DataContext as RouteReportDataGridViewModel;

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            await ViewModel.LoadDataAsync();
        }
    }
}
