namespace TrainsOnline.Desktop.Views.User
{
    using TrainsOnline.Desktop.ViewModels.User;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Navigation;

    public sealed partial class UserDetailsPage : Page
    {
        public UserDetailsPage()
        {
            InitializeComponent();
        }

        private UserDetailsViewModel ViewModel => DataContext as UserDetailsViewModel;

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            await ViewModel.InitializeAsync();
        }
    }
}
