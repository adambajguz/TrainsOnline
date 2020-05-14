namespace TrainsOnline.Desktop.Views.Example
{
    using System.Linq;
    using Microsoft.Toolkit.Uwp.UI.Controls;
    using TrainsOnline.Desktop.ViewModels.Example;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;

    public sealed partial class ExampleMasterDetailPage : Page
    {
        public ExampleMasterDetailPage()
        {
            InitializeComponent();
        }

        private ExampleMasterDetailViewModel ViewModel => DataContext as ExampleMasterDetailViewModel;

        private void MasterDetailsViewControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (MasterDetailsViewControl.ViewState == MasterDetailsViewState.Both)
            {
                ViewModel.ActiveItem = ViewModel.Items.FirstOrDefault();
            }
        }
    }
}
