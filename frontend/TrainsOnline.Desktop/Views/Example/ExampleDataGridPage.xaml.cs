namespace TrainsOnline.Desktop.Views.Example
{
    using TrainsOnline.Desktop.ViewModels.Example;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Navigation;

    public sealed partial class ExampleDataGridPage : Page
    {
        // TODO WTS: Change the grid as appropriate to your app, adjust the column definitions on ExampleDataGridPage.xaml.
        // For more details see the documentation at https://docs.microsoft.com/windows/communitytoolkit/controls/datagrid
        public ExampleDataGridPage()
        {
            InitializeComponent();
        }

        private ExampleDataGridViewModel ViewModel => DataContext as ExampleDataGridViewModel;

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            await ViewModel.LoadDataAsync();
        }
    }
}
