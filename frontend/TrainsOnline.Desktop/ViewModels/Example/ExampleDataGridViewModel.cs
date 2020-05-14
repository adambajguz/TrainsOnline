namespace TrainsOnline.Desktop.ViewModels.Example
{
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    using Caliburn.Micro;
    using TrainsOnline.Desktop.Application.Services;
    using TrainsOnline.Desktop.Domain.Models;

    public class ExampleDataGridViewModel : Screen
    {
        public ObservableCollection<SampleOrder> Source { get; } = new ObservableCollection<SampleOrder>();

        public ExampleDataGridViewModel()
        {
        }

        public async Task LoadDataAsync()
        {
            Source.Clear();

            // TODO WTS: Replace this with your actual data
            System.Collections.Generic.IEnumerable<SampleOrder> data = await SampleDataService.GetGridDataAsync();

            foreach (SampleOrder item in data)
            {
                Source.Add(item);
            }
        }
    }
}
