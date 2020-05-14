namespace TrainsOnline.Desktop.Views.Station.StationMasterDetailDetail
{
    using TrainsOnline.Desktop.ViewModels.Station;

    public sealed partial class DetailsView
    {
        public DetailsView()
        {
            InitializeComponent();
        }

        public StationMasterDetailDetailViewModel ViewModel => DataContext as StationMasterDetailDetailViewModel;
    }
}
