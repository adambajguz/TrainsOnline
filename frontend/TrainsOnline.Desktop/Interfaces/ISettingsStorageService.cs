namespace TrainsOnline.Desktop.Interfaces
{
    using System.Threading.Tasks;

    public interface ISettingsStorageService
    {
        Task<string> LoadFromSettingsAsync(string key);
        Task SaveInSettingsAsync(string key, string value);
    }
}
