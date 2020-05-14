namespace TrainsOnline.Desktop.ViewModels
{
    using TrainsOnline.Desktop.Application.Interfaces.RemoteDataProvider;
    using Windows.UI.Xaml;

    public interface ISettingsViewModel
    {
        void SwitchApi();
        void SwitchApiUrl();
        void SwitchApiVersion(WebApiTypes apiType);
        void SwitchTheme(ElementTheme theme);
    }
}
