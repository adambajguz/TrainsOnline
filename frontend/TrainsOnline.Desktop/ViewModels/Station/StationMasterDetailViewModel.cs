namespace TrainsOnline.Desktop.ViewModels.Station
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Caliburn.Micro;
    using TrainsOnline.Desktop.Application.Interfaces.RemoteDataProvider;
    using TrainsOnline.Desktop.Domain.DTO.Station;

    public class StationMasterDetailViewModel : Conductor<StationMasterDetailDetailViewModel>.Collection.OneActive
    {
        private INavigationService NavService { get; }
        private IRemoteDataProviderService RemoteDataProvider { get; }

        public StationMasterDetailViewModel(INavigationService navigationService,
                                            IRemoteDataProviderService remoteDataProvider)
        {
            NavService = navigationService;
            RemoteDataProvider = remoteDataProvider;
        }

        protected override async void OnInitialize()
        {
            base.OnInitialize();

            await LoadDataAsync();
        }

        public async Task LoadDataAsync()
        {
            Items.Clear();

            GetStationsListResponse data = await RemoteDataProvider.GetStations();

            IEnumerable<StationMasterDetailDetailViewModel> items = data.Stations.OrderBy(x => x.Name)
                                                                                 .Select(d => new StationMasterDetailDetailViewModel(NavService, RemoteDataProvider, d));

            Items.AddRange(items);
        }

        public override async void ActivateItem(StationMasterDetailDetailViewModel item)
        {
            if (item is null)
                return;

            base.ActivateItem(item);

            await item.LoadDetails();
        }
    }
}
