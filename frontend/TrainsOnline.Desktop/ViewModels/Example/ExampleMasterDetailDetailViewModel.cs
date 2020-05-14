namespace TrainsOnline.Desktop.ViewModels.Example
{
    using Caliburn.Micro;
    using TrainsOnline.Desktop.Domain.Models;

    public class ExampleMasterDetailDetailViewModel : Screen
    {
        public ExampleMasterDetailDetailViewModel(SampleOrder item)
        {
            Item = item;
        }

        public SampleOrder Item { get; }
    }
}
