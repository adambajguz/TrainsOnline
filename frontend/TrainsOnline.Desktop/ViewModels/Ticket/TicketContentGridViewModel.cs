namespace TrainsOnline.Desktop.ViewModels.Ticket
{
    using System;
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    using Caliburn.Micro;
    using TrainsOnline.Desktop.Application.Interfaces.RemoteDataProvider;
    using TrainsOnline.Desktop.Domain.DTO.Ticket;
    using TrainsOnline.Desktop.Domain.Models.Ticket;
    using TrainsOnline.Desktop.Interfaces;
    using TrainsOnline.Desktop.ViewModels.User;
    using TrainsOnline.Desktop.Views.Ticket;

    public class TicketContentGridViewModel : Screen, ITicketContentGridViewEvents
    {
        private readonly INavigationService _navigationService;
        private readonly IConnectedAnimationService _connectedAnimationService;
        private IRemoteDataProviderService RemoteDataProvider { get; }

        public ObservableCollection<UserTicketLookupModel> Source { get; } = new ObservableCollection<UserTicketLookupModel>();

        public TicketContentGridViewModel(INavigationService navigationService,
                                          IConnectedAnimationService connectedAnimationService,
                                          IRemoteDataProviderService remoteDataProvider)
        {
            _navigationService = navigationService;
            _connectedAnimationService = connectedAnimationService;
            RemoteDataProvider = remoteDataProvider;
        }

        public async Task LoadDataAsync()
        {
            if (!RemoteDataProvider.IsAuthenticated)
            {
                _navigationService.NavigateToViewModel<LoginRegisterViewModel>();

                return;
            }
            try
            {

                Source.Clear();

                GetUserTicketsListResponse data = await RemoteDataProvider.GetCurrentUserTickets();

                if (data is null)
                    return;

                foreach (UserTicketLookupModel ticket in data.Tickets)
                {
                    Source.Add(ticket);
                }
            }
            catch (Exception)
            {

            }
        }

        public void OnItemSelected(UserTicketLookupModel clickedItem)
        {
            if (clickedItem != null)
            {
                _connectedAnimationService.SetListDataItemForNextConnectedAnimation(clickedItem);
                _navigationService.NavigateToViewModel<TicketContentGridDetailViewModel>(new TicketContentGridDetailsParameters(clickedItem.Id));
            }
        }

        protected override void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);

            if (!RemoteDataProvider.IsAuthenticated)
            {
                _navigationService.NavigateToViewModel<LoginRegisterViewModel>();
            }
        }
    }
}
