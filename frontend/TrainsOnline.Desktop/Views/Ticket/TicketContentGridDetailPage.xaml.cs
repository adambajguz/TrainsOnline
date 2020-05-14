namespace TrainsOnline.Desktop.Views.Ticket
{
    using TrainsOnline.Desktop.ViewModels.Ticket;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Navigation;

    public sealed partial class TicketContentGridDetailPage : Page, ITicketContentGridDetailView
    {
        public TicketContentGridDetailPage()
        {
            InitializeComponent();
        }

        private TicketContentGridDetailViewModel ViewModel => DataContext as TicketContentGridDetailViewModel;

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            await ViewModel.InitializeAsync();
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
            if (e.NavigationMode == NavigationMode.Back)
            {
                ViewModel.SetListDataItemForNextConnectedAnimation();
            }
        }

        //public void SetImage(ImageSource imageSource)
        //{
        //    PdfRenderingImage.Source = imageSource;
        //}
    }
}
