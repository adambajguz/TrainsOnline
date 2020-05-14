namespace TrainsOnline.Desktop.Views
{
    using TrainsOnline.Desktop.ViewModels;
    using Windows.UI.Xaml.Controls;

    // TODO WTS: Change the URL for your privacy policy in the Resource File, currently set to https://YourPrivacyUrlGoesHere
    public sealed partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            InitializeComponent();
        }

        private SettingsViewModel ViewModel => DataContext as SettingsViewModel;
    }
}
