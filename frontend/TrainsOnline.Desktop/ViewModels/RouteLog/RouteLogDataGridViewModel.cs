﻿namespace TrainsOnline.Desktop.ViewModels.RouteLog
{
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using Caliburn.Micro;
    using Microsoft.Toolkit.Uwp.UI.Controls;
    using TrainsOnline.Desktop.Application.Exceptions;
    using TrainsOnline.Desktop.Application.Interfaces.RemoteDataProvider;
    using TrainsOnline.Desktop.Domain.DTO.EntityAuditLog;
    using TrainsOnline.Desktop.Domain.DTO.RouteLog;
    using TrainsOnline.Desktop.Helpers;
    using TrainsOnline.Desktop.ViewModels.User;
    using TrainsOnline.Desktop.Views.RouteLog;
    using Windows.UI.Xaml.Data;

    public class RouteLogDataGridViewModel : Screen, IRouteLogDataGridViewEvents
    {
        private INavigationService NavService { get; }
        private IRemoteDataProviderService RemoteDataProvider { get; }

        public ObservableCollection<GroupInfoCollection<RouteLogLookupModel>> Source { get; } = new ObservableCollection<GroupInfoCollection<RouteLogLookupModel>>();
        public CollectionViewSource GroupedSource { get; } = new CollectionViewSource();

        public RouteLogDataGridViewModel(INavigationService navigationService,
                                      IRemoteDataProviderService remoteDataProvider)
        {
            NavService = navigationService;
            RemoteDataProvider = remoteDataProvider;
        }

        public async Task LoadDataAsync()
        {
            if (!RemoteDataProvider.IsAuthenticated)
            {
                NavService.NavigateToViewModel<LoginRegisterViewModel>();

                return;
            }

            try
            {
                GetRouteLogsListResponse data = await RemoteDataProvider.GetEntityRouteLogs();

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

            if (!RemoteDataProvider.IsAuthenticated)
            {
                NavService.NavigateToViewModel<LoginRegisterViewModel>();

                return;
            }
        }

        private void LoadDataAsync(GetRouteLogsListResponse data)
        {
            if (data is null)
                return;

            Source.Clear();

            var query = from item in data.RouteLogs
                        orderby item.Timestamp descending
                        group item by item.RouteId into g
                        select new { GroupName = g.Key, Items = g };

            foreach (var g in query)
            {
                GroupInfoCollection<RouteLogLookupModel> info = new GroupInfoCollection<RouteLogLookupModel>
                {
                    Key = g.GroupName
                };

                foreach (RouteLogLookupModel item in g.Items)
                    info.Add(item);

                Source.Add(info);
            }

            GroupedSource.IsSourceGrouped = true;
            GroupedSource.Source = Source;
        }

        public void LoadingRowGroup(DataGridRowGroupHeaderEventArgs e)
        {
            ICollectionViewGroup group = e.RowGroupHeader.CollectionViewGroup;
            RouteLogLookupModel item = group.GroupItems[0] as RouteLogLookupModel;
            e.RowGroupHeader.PropertyValue = item?.RouteId.ToString();
        }

        public async void ResetView()
        {
            await LoadDataAsync();
        }
    }
}
