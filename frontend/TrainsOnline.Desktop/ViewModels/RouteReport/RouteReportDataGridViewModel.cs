namespace TrainsOnline.Desktop.ViewModels.RouteReport
{
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using Caliburn.Micro;
    using Microsoft.Toolkit.Uwp.UI.Controls;
    using TrainsOnline.Desktop.Application.Exceptions;
    using TrainsOnline.Desktop.Application.Interfaces.RemoteDataProvider;
    using TrainsOnline.Desktop.Domain.DTO.RouteReport;
    using TrainsOnline.Desktop.Helpers;
    using TrainsOnline.Desktop.ViewModels.User;
    using TrainsOnline.Desktop.Views.RouteReport;
    using Windows.UI.Xaml.Data;

    public class RouteReportDataGridViewModel : Screen, IRouteReportDataGridViewEvents
    {
        private INavigationService NavService { get; }
        private IRemoteDataProviderService RemoteDataProvider { get; }

        public ObservableCollection<GroupInfoCollection<RouteReportLookupModel>> Source { get; } = new ObservableCollection<GroupInfoCollection<RouteReportLookupModel>>();
        public CollectionViewSource GroupedSource { get; } = new CollectionViewSource();

        public RouteReportDataGridViewModel(INavigationService navigationService,
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
                GetRouteReportsListResponse data = await RemoteDataProvider.GetEntityRouteReports();

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

        private void LoadDataAsync(GetRouteReportsListResponse data)
        {
            if (data is null)
                return;

            Source.Clear();

            var query = from item in data.RouteReports
                        orderby item.CreatedOn descending
                        group item by item.RouteId into g
                        select new { GroupName = g.Key, Items = g };

            foreach (var g in query)
            {
                GroupInfoCollection<RouteReportLookupModel> info = new GroupInfoCollection<RouteReportLookupModel>
                {
                    Key = g.GroupName
                };

                foreach (RouteReportLookupModel item in g.Items)
                    info.Add(item);

                Source.Add(info);
            }

            GroupedSource.IsSourceGrouped = true;
            GroupedSource.Source = Source;
        }

        public void LoadingRowGroup(DataGridRowGroupHeaderEventArgs e)
        {
            ICollectionViewGroup group = e.RowGroupHeader.CollectionViewGroup;
            RouteReportLookupModel item = group.GroupItems[0] as RouteReportLookupModel;
            e.RowGroupHeader.PropertyValue = item?.RouteId.ToString();
        }

        public async void ResetView()
        {
            await LoadDataAsync();
        }
    }
}
