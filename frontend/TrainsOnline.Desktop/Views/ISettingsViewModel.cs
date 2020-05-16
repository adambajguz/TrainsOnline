namespace TrainsOnline.Desktop.ViewModels
{
    using Windows.UI.Xaml;

    public interface ISettingsViewModel
    {
        void SwitchApiUrl();
        void SwitchTheme(ElementTheme theme);
    }
}
