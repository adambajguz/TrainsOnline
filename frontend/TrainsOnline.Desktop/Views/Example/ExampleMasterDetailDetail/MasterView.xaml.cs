namespace TrainsOnline.Desktop.Views.Example.ExampleMasterDetailDetail
{
    using TrainsOnline.Desktop.ViewModels.Example;

    public sealed partial class MasterView
    {
        public MasterView()
        {
            InitializeComponent();
        }

        public ExampleMasterDetailDetailViewModel ViewModel => DataContext as ExampleMasterDetailDetailViewModel;
    }
}
