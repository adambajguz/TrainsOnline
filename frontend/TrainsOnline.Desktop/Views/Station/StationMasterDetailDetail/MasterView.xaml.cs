namespace TrainsOnline.Desktop.Views.Station.StationMasterDetailDetail
{
    using TrainsOnline.Desktop.ViewModels.Station;

    public sealed partial class MasterView
    {
        public MasterView()
        {
            InitializeComponent();
        }

        public StationMasterDetailDetailViewModel ViewModel => DataContext as StationMasterDetailDetailViewModel;
    }
}
