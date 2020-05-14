namespace TrainsOnline.Desktop.Services.File
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using TrainsOnline.Desktop.Interfaces;
    using Windows.Storage;
    using Windows.Storage.Pickers;

    public class FileService : IFileService
    {
        public FileService()
        {

        }

        public async Task<FileSavingResults> SaveToPdfFile(byte[] content, string suggestedName)
        {
            FileSavePicker savePicker = new FileSavePicker
            {
                SuggestedStartLocation = PickerLocationId.Downloads
            };

            savePicker.FileTypeChoices.Add("PDF Document", new List<string>() { ".pdf" });
            savePicker.SuggestedFileName = suggestedName;

            StorageFile file = await savePicker.PickSaveFileAsync();
            if (file != null)
            {
                // Prevent updates to the remote version of the file until
                // we finish making changes and call CompleteUpdatesAsync.
                CachedFileManager.DeferUpdates(file);

                await FileIO.WriteBytesAsync(file, content);

                // Let Windows know that we're finished changing the file so
                // the other app can update the remote version of the file.
                // Completing updates may require Windows to ask for user input.
                Windows.Storage.Provider.FileUpdateStatus status = await CachedFileManager.CompleteUpdatesAsync(file);

                if (status != Windows.Storage.Provider.FileUpdateStatus.Complete)
                    return FileSavingResults.Error;
            }
            else
            {
                return FileSavingResults.Cancelled;
            }

            return FileSavingResults.Ok;
        }
    }
}
