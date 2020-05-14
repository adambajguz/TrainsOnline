namespace TrainsOnline.Desktop.Services
{
    using System.Threading.Tasks;
    using TrainsOnline.Desktop.Helpers;
    using TrainsOnline.Desktop.Interfaces;
    using Windows.Storage;

    public class SettingStorageService : ISettingsStorageService
    {
        public SettingStorageService()
        {

        }
        public async Task<string> LoadFromSettingsAsync(string key)
        {
            return await ApplicationData.Current.LocalSettings.ReadAsync<string>(key);
        }

        public async Task SaveInSettingsAsync(string key, string value)
        {
            await ApplicationData.Current.LocalSettings.SaveAsync(key, value);
        }
    }
}
