namespace TrainsOnline.Desktop.Views.Ticket
{
    using TrainsOnline.Desktop.ViewModels.Ticket;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Navigation;

    public sealed partial class TicketContentGridPage : Page
    {
        public TicketContentGridPage()
        {
            InitializeComponent();
        }

        private TicketContentGridViewModel ViewModel => DataContext as TicketContentGridViewModel;

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            await ViewModel.LoadDataAsync();
        }
    }
}
