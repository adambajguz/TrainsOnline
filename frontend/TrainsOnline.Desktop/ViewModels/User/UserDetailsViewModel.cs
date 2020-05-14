namespace TrainsOnline.Desktop.ViewModels.User
{
    using System.Threading.Tasks;
    using Caliburn.Micro;
    using TrainsOnline.Desktop.Application.Exceptions;
    using TrainsOnline.Desktop.Application.Interfaces.RemoteDataProvider;
    using TrainsOnline.Desktop.Domain.DTO.User;
    using TrainsOnline.Desktop.Views.User;

    public class UserDetailsViewModel : Screen, IUserDetailsViewEvents
    {
        private readonly INavigationService _navigationService;
        private IRemoteDataProviderService RemoteDataProvider { get; }

        private GetUserDetailsResponse _item;
        public GetUserDetailsResponse Item
        {
            get => _item;
            set => Set(ref _item, value);
        }

        private string _editErrors;
        public string EditErrors
        {
            get => _editErrors;
            set => Set(ref _editErrors, value);
        }

        private bool _editInProgress;
        public bool EditInProgress
        {
            get => _editInProgress;
            set => Set(ref _editInProgress, value);
        }

        private string _passwordChangeErrors;
        public string PasswordChangeErrors
        {
            get => _passwordChangeErrors;
            set => Set(ref _passwordChangeErrors, value);
        }

        private bool _passwordChangeInProgress;
        public bool PasswordChangeInProgress
        {
            get => _passwordChangeInProgress;
            set => Set(ref _passwordChangeInProgress, value);
        }

        private string _currentPassword;
        public string CurrentPassword
        {
            get => _currentPassword;
            set => Set(ref _currentPassword, value);
        }

        private string _newPassword;
        public string NewPassword
        {
            get => _newPassword;
            set => Set(ref _newPassword, value);
        }

        public UserDetailsViewModel(INavigationService navigationService,
                                    IRemoteDataProviderService remoteDataProvider)
        {
            _navigationService = navigationService;
            RemoteDataProvider = remoteDataProvider;
        }

        public async Task InitializeAsync()
        {
            if (!RemoteDataProvider.IsAuthenticated)
            {
                _navigationService.NavigateToViewModel<LoginRegisterViewModel>();

                return;
            }

            try
            {
                Item = await RemoteDataProvider.GetCurrentUser();
            }
            catch (RemoteDataException)
            {

            }
        }

        public async void UpdateDetails()
        {
            EditInProgress = true;
            try
            {
                await RemoteDataProvider.UpdateUser(new UpdateUserRequest
                {
                    Id = Item.Id,
                    Email = Item.Email,
                    Name = Item.Name,
                    Surname = Item.Surname,
                    Address = Item.Address,
                    PhoneNumber = Item.PhoneNumber,
                    IsAdmin = Item.IsAdmin
                });
                EditErrors = "";
            }
            catch (RemoteDataException ex)
            {
                EditErrors = ex.GetResponse().Message;
            }
            EditInProgress = false;
        }

        public void Logout()
        {
            RemoteDataProvider.Logout();
            _navigationService.NavigateToViewModel<LoginRegisterViewModel>();
        }

        protected override void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);

            if (!RemoteDataProvider.IsAuthenticated)
            {
                _navigationService.NavigateToViewModel<LoginRegisterViewModel>();
            }
        }

        public async void UpdatePassword()
        {
            PasswordChangeInProgress = true;
            try
            {
                await RemoteDataProvider.ChangePassword(CurrentPassword, NewPassword);
                PasswordChangeErrors = "";
            }
            catch (RemoteDataException ex)
            {
                PasswordChangeErrors = ex.GetResponse().Message;
            }
            PasswordChangeInProgress = false;
        }
    }
}
