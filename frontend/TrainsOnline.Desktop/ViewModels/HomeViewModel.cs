namespace TrainsOnline.Desktop.ViewModels
{
    using Caliburn.Micro;
    using TrainsOnline.Desktop.Application.Interfaces.RemoteDataProvider;
    using TrainsOnline.Desktop.Constants;
    using TrainsOnline.Desktop.Interfaces;

    public class HomeViewModel : Screen
    {
        private IRemoteDataProviderService RemoteDataProvider { get; }
        private ISettingsStorageService SettingsStorage { get; }


        public HomeViewModel(ISettingsStorageService settingsStorage,
                                 IRemoteDataProviderService remoteDataProvider)
        {
            SettingsStorage = settingsStorage;
            RemoteDataProvider = remoteDataProvider;

        }

        protected override async void OnInitialize()
        {
            base.OnInitialize();

            string apiUrltype = await SettingsStorage.LoadFromSettingsAsync(SettingKeys.ApiUseLocalSettingKey);
            bool.TryParse(apiUrltype, out bool useLocalApi);
            RemoteDataProvider.UseLocalUrl = useLocalApi;
        }
    }
}
