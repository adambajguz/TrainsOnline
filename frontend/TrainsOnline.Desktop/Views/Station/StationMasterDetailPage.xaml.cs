namespace TrainsOnline.Desktop.Views.Station
{
    using System.Linq;
    using Microsoft.Toolkit.Uwp.UI.Controls;
    using TrainsOnline.Desktop.ViewModels.Station;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;

    public sealed partial class StationMasterDetailPage : Page
    {
        public StationMasterDetailPage()
        {
            InitializeComponent();
        }

        private StationMasterDetailViewModel ViewModel => DataContext as StationMasterDetailViewModel;

        private void MasterDetailsViewControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (MasterDetailsViewControl.ViewState == MasterDetailsViewState.Both)
            {
                ViewModel.ActiveItem = ViewModel.Items.FirstOrDefault();
            }
        }
    }
}
