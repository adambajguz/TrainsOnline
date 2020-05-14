namespace TrainsOnline.Desktop.Views.Example.ExampleMasterDetailDetail
{
    using TrainsOnline.Desktop.ViewModels.Example;

    public sealed partial class DetailsView
    {
        public DetailsView()
        {
            InitializeComponent();
        }

        public ExampleMasterDetailDetailViewModel ViewModel => DataContext as ExampleMasterDetailDetailViewModel;
    }
}
