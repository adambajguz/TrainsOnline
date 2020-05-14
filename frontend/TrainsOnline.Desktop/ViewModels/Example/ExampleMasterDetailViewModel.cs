namespace TrainsOnline.Desktop.ViewModels.Example
{
    using System.Linq;
    using System.Threading.Tasks;
    using Caliburn.Micro;
    using TrainsOnline.Desktop.Application.Services;

    public class ExampleMasterDetailViewModel : Conductor<ExampleMasterDetailDetailViewModel>.Collection.OneActive
    {
        protected override async void OnInitialize()
        {
            base.OnInitialize();

            await LoadDataAsync();
        }

        public async Task LoadDataAsync()
        {
            Items.Clear();

            System.Collections.Generic.IEnumerable<Domain.Models.SampleOrder> data = await SampleDataService.GetMasterDetailDataAsync();

            Items.AddRange(data.Select(d => new ExampleMasterDetailDetailViewModel(d)));
        }
    }
}
