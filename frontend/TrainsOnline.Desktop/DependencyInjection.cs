namespace TrainsOnline.Desktop
{
    using Caliburn.Micro;
    using TrainsOnline.Desktop.Interfaces;
    using TrainsOnline.Desktop.Services;
    using TrainsOnline.Desktop.Services.File;
    using VM = ViewModels;

    public static class DependencyInjection
    {
        public static void AddPresentation(this WinRTContainer _container)
        {
            _container.RegisterWinRTServices();

            _container.PerRequest<VM.ShellViewModel>();
            _container.PerRequest<VM.HomeViewModel>();
            _container.PerRequest<VM.SettingsViewModel>();

            _container.PerRequest<VM.General.GeneralMapViewModel>();

            _container.PerRequest<VM.Example.ExampleBlankViewModel>();
            _container.PerRequest<VM.Example.ExampleMasterDetailViewModel>();
            _container.PerRequest<VM.Example.ExampleContentGridDetailViewModel>();
            _container.PerRequest<VM.Example.ExampleContentGridViewModel>();
            _container.PerRequest<VM.Example.ExampleDataGridViewModel>();
            _container.PerRequest<VM.Example.ExampleMapViewModel>();

            _container.PerRequest<VM.Route.RouteDataGridViewModel>();

            _container.PerRequest<VM.Station.StationMasterDetailViewModel>();

            _container.PerRequest<VM.Ticket.TicketContentGridDetailViewModel>();
            _container.PerRequest<VM.Ticket.TicketContentGridViewModel>();

            _container.PerRequest<VM.User.LoginRegisterViewModel>();
            _container.PerRequest<VM.User.UserDetailsViewModel>();

            _container.PerRequest<VM.Admin.AdminAnalyticsDataGridViewModel>();
            _container.PerRequest<VM.Admin.AdminEntityAuditLogDataGridViewModel>();

            _container.Singleton<IFileService, FileService>();
            _container.Singleton<ISettingsStorageService, SettingStorageService>();
        }
    }
}
