namespace TrainsOnline.Desktop.Services
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Toolkit.Uwp.Helpers;
    using TrainsOnline.Desktop.Views;
    using Windows.ApplicationModel.Core;
    using Windows.UI.Core;

    public static class FirstRunDisplayService
    {
        private static bool shown = false;

        internal static async Task ShowIfAppropriateAsync()
        {
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
                CoreDispatcherPriority.Normal, async () =>
                {
                    if (SystemInformation.IsFirstRun && !shown)
                    {
                        shown = true;
                        FirstRunDialog dialog = new FirstRunDialog();
                        await dialog.ShowAsync();
                    }
                });
        }
    }
}
