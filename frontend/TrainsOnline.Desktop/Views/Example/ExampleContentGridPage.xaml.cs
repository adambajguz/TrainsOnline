namespace TrainsOnline.Desktop.Views.Example
{
    using TrainsOnline.Desktop.ViewModels.Example;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Navigation;

    public sealed partial class ExampleContentGridPage : Page
    {
        public ExampleContentGridPage()
        {
            InitializeComponent();
        }

        private ExampleContentGridViewModel ViewModel => DataContext as ExampleContentGridViewModel;

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            await ViewModel.LoadDataAsync();
        }
    }
}
