namespace TrainsOnline.Desktop.ViewModels.Admin
{
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using Caliburn.Micro;
    using Microsoft.Toolkit.Uwp.UI.Controls;
    using TrainsOnline.Desktop.Application.Exceptions;
    using TrainsOnline.Desktop.Application.Interfaces.RemoteDataProvider;
    using TrainsOnline.Desktop.Domain.DTO.Analytics;
    using TrainsOnline.Desktop.Helpers;
    using TrainsOnline.Desktop.ViewModels.User;
    using TrainsOnline.Desktop.Views.Route;
    using Windows.UI.Xaml.Data;

    public class AdminAnalyticsDataGridViewModel : Screen, IAdminAnalyticsDataGridViewEvents
    {
        private INavigationService NavService { get; }
        private IRemoteDataProviderService RemoteDataProvider { get; }

        public ObservableCollection<GroupInfoCollection<AnalyticsRecordLookupModel>> Source { get; } = new ObservableCollection<GroupInfoCollection<AnalyticsRecordLookupModel>>();
        public CollectionViewSource GroupedSource { get; } = new CollectionViewSource();

        public AdminAnalyticsDataGridViewModel(INavigationService navigationService,
                                      IRemoteDataProviderService remoteDataProvider)
        {
            NavService = navigationService;
            RemoteDataProvider = remoteDataProvider;
        }

        public async Task LoadDataAsync()
        {
            if (!RemoteDataProvider.IsAuthenticated || !RemoteDataProvider.HasRole("Admin"))
            {
                RemoteDataProvider.Logout();
                NavService.NavigateToViewModel<LoginRegisterViewModel>();

                return;
            }

            try
            {
                GetAnalyticsRecordsListResponse data = await RemoteDataProvider.GetAnalytics();

                //foreach (RouteLookupModel route in data.Routes)
                //{
                //    GetRouteDetailsResponse details = await RemoteDataProvider.GetRoute(route.Id);

                //    Source.Add(info);
                //}

                LoadDataAsync(data);
            }
            catch (RemoteDataException)
            {

            }
        }

        protected override void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);

            if (!RemoteDataProvider.IsAuthenticated || !RemoteDataProvider.HasRole("Admin"))
            {
                RemoteDataProvider.Logout();
                NavService.NavigateToViewModel<LoginRegisterViewModel>();

                return;
            }
        }

        private void LoadDataAsync(GetAnalyticsRecordsListResponse data)
        {
            if (data is null)
            {
                return;
            }

            Source.Clear();

            var query = from item in data.AnalyticsRecords
                        group item by item.Uri into g
                        select new { GroupName = g.Key, Items = g };

            foreach (var g in query)
            {
                GroupInfoCollection<AnalyticsRecordLookupModel> info = new GroupInfoCollection<AnalyticsRecordLookupModel>
                {
                    Key = g.GroupName
                };

                foreach (AnalyticsRecordLookupModel item in g.Items)
                {
                    info.Add(item);
                }

                Source.Add(info);
            }

            GroupedSource.IsSourceGrouped = true;
            GroupedSource.Source = Source;
        }

        public void LoadingRowGroup(DataGridRowGroupHeaderEventArgs e)
        {
            ICollectionViewGroup group = e.RowGroupHeader.CollectionViewGroup;
            AnalyticsRecordLookupModel item = group.GroupItems[0] as AnalyticsRecordLookupModel;
            e.RowGroupHeader.PropertyValue = item?.Uri;
        }

        public async void ResetView()
        {
            await LoadDataAsync();
        }
    }
}
