namespace TrainsOnline.Desktop.Views
{
    using TrainsOnline.Desktop.ViewModels;
    using Windows.UI.Xaml.Controls;

    public sealed partial class HomePage : Page
    {
        public HomePage()
        {
            InitializeComponent();
        }

        private HomeViewModel ViewModel => DataContext as HomeViewModel;
    }
}
