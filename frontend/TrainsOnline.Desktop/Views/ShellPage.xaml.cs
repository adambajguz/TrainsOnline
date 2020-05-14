namespace TrainsOnline.Desktop.Views
{
    using Caliburn.Micro;
    using TrainsOnline.Desktop.ViewModels;
    using Windows.UI.Xaml.Controls;
    using WinUI = Microsoft.UI.Xaml.Controls;

    // TODO WTS: Change the icons and titles for all NavigationViewItems in ShellPage.xaml.
    public sealed partial class ShellPage : IShellView
    {
        private ShellViewModel ViewModel => DataContext as ShellViewModel;

        public ShellPage()
        {
            InitializeComponent();
        }

        public INavigationService CreateNavigationService(WinRTContainer container)
        {
            INavigationService navigationService = container.RegisterNavigationService(ShellFrame);
            navigationViewHeaderBehavior.Initialize(navigationService);
            return navigationService;
        }

        public WinUI.NavigationView GetNavigationView()
        {
            return navigationView;
        }

        public Frame GetFrame()
        {
            return ShellFrame;
        }
    }
}
