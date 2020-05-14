namespace TrainsOnline.Desktop.ViewModels.Example
{
    using System.Linq;
    using System.Threading.Tasks;
    using Caliburn.Micro;
    using TrainsOnline.Desktop.Application.Services;
    using TrainsOnline.Desktop.Domain.Models;
    using TrainsOnline.Desktop.Interfaces;

    public class ExampleContentGridDetailViewModel : Screen
    {
        private readonly IConnectedAnimationService _connectedAnimationService;

        private SampleOrder _item;

        public SampleOrder Item
        {
            get => _item;
            set => Set(ref _item, value);
        }

        public ExampleContentGridDetailViewModel(IConnectedAnimationService connectedAnimationService)
        {
            _connectedAnimationService = connectedAnimationService;
        }

        public async Task InitializeAsync(long orderID)
        {
            // TODO WTS: Replace this with your actual data
            System.Collections.Generic.IEnumerable<SampleOrder> data = await SampleDataService.GetContentGridDataAsync();
            Item = data.First(i => i.OrderID == orderID);
        }

        public void SetListDataItemForNextConnectedAnimation()
        {
            _connectedAnimationService.SetListDataItemForNextConnectedAnimation(Item);
        }
    }
}
