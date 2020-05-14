namespace TrainsOnline.Desktop.Views.User
{
    using TrainsOnline.Desktop.ViewModels.User;
    using Windows.UI.Xaml.Controls;

    public sealed partial class LoginRegisterPage : Page
    {
        public LoginRegisterPage()
        {
            InitializeComponent();
        }

        private LoginRegisterViewModel ViewModel => DataContext as LoginRegisterViewModel;
    }
}
