namespace TrainsOnline.Desktop.Views.Admin
{
    using TrainsOnline.Desktop.ViewModels.Admin;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Navigation;

    public sealed partial class AdminEntityAuditLogDataGridPage : Page, IAdminEntityAuditLogDataGridView
    {
        // TODO WTS: Change the grid as appropriate to your app, adjust the column definitions on ExampleDataGridPage.xaml.
        // For more details see the documentation at https://docs.microsoft.com/windows/communitytoolkit/controls/datagrid
        public AdminEntityAuditLogDataGridPage()
        {
            InitializeComponent();
        }

        private AdminEntityAuditLogDataGridViewModel ViewModel => DataContext as AdminEntityAuditLogDataGridViewModel;

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            await ViewModel.LoadDataAsync();
        }
    }
}
