namespace TrainsOnline.Desktop.ViewModels
{
    using Caliburn.Micro;
    using TrainsOnline.Desktop.Application.Interfaces.RemoteDataProvider;
    using TrainsOnline.Desktop.Constants;
    using TrainsOnline.Desktop.Helpers;
    using TrainsOnline.Desktop.Interfaces;
    using TrainsOnline.Desktop.Services;
    using Windows.ApplicationModel;
    using Windows.UI.Xaml;

    // TODO WTS: Add other settings as necessary. For help see https://github.com/Microsoft/WindowsTemplateStudio/blob/master/docs/pages/settings.md
    public class SettingsViewModel : Screen, ISettingsViewModel
    {
        private ElementTheme _elementTheme = ThemeSelectorService.Theme;
        public ElementTheme ElementTheme
        {
            get => _elementTheme;
            set => Set(ref _elementTheme, value);
        }

        private bool _useLocalApi;
        public bool UseLocalApi
        {
            get => _useLocalApi;
            set => Set(ref _useLocalApi, value);
        }

        private string _versionDescription;
        public string VersionDescription
        {
            get => _versionDescription;
            set => Set(ref _versionDescription, value);
        }

        private IRemoteDataProviderService RemoteDataProvider { get; }
        private ISettingsStorageService SettingsStorage { get; }

        public SettingsViewModel(ISettingsStorageService settingsStorage,
                                 IRemoteDataProviderService remoteDataProvider)
        {
            SettingsStorage = settingsStorage;
            RemoteDataProvider = remoteDataProvider;
        }

        private bool DontFireEvents { get; set; } = true;

        protected override async void OnInitialize()
        {
            base.OnInitialize();

            VersionDescription = GetVersionDescription();

            string apiUrltype = await SettingsStorage.LoadFromSettingsAsync(SettingKeys.ApiUseLocalSettingKey);
            bool.TryParse(apiUrltype, out bool useLocalApi);
            _useLocalApi = useLocalApi;
            RemoteDataProvider.UseLocalUrl = useLocalApi;

            Refresh();

            DontFireEvents = false;
        }

        private string GetVersionDescription()
        {
            string appName = "AppDisplayName".GetLocalized();
            Package package = Package.Current;
            PackageId packageId = package.Id;
            PackageVersion version = packageId.Version;

            return $"{appName} - {version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
        }

        public async void SwitchTheme(ElementTheme theme)
        {
            if (DontFireEvents)
                return;

            await ThemeSelectorService.SetThemeAsync(theme);
        }

        public async void SwitchApiUrl()
        {
            if (DontFireEvents)
                return;

            bool value = UseLocalApi;
            RemoteDataProvider.UseLocalUrl = value;
            await SettingsStorage.SaveInSettingsAsync(SettingKeys.ApiUseLocalSettingKey, value.ToString());
        }
    }
}
