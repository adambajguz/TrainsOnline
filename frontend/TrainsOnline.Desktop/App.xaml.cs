namespace TrainsOnline.Desktop
{
    using System;
    using System.Collections.Generic;
    using Caliburn.Micro;
    using TrainsOnline.Desktop.Core;
    using TrainsOnline.Desktop.Infrastructure;
    using TrainsOnline.Desktop.Interfaces;
    using TrainsOnline.Desktop.Services;
    using Windows.ApplicationModel.Activation;
    using Windows.UI.Xaml;

    [Windows.UI.Xaml.Data.Bindable]
    public sealed partial class App
    {
        private readonly Lazy<ActivationService> _activationService;

        private ActivationService ActivationService => _activationService.Value;

        public App()
        {
            InitializeComponent();

            Initialize();

            // Deferred execution until used. Check https://msdn.microsoft.com/library/dd642331(v=vs.110).aspx for further info on Lazy<T> class.
            _activationService = new Lazy<ActivationService>(CreateActivationService);
        }

        protected override async void OnLaunched(LaunchActivatedEventArgs args)
        {
            if (!args.PrelaunchActivated)
            {
                await ActivationService.ActivateAsync(args);
            }
        }

        protected override async void OnActivated(IActivatedEventArgs args)
        {
            await ActivationService.ActivateAsync(args);
        }

        private WinRTContainer _container;

        protected override void Configure()
        {
            // This configures the framework to map between MainViewModel and MainPage
            // Normally it would map between MainPageViewModel and MainPage
            TypeMappingConfiguration config = new TypeMappingConfiguration
            {
                IncludeViewSuffixInViewModelNames = false
            };

            ViewLocator.ConfigureTypeMappings(config);
            ViewModelLocator.ConfigureTypeMappings(config);

            // ViewLocator.AddNamespaceMapping("TrainsOnline.Desktop.Views.*", "TrainsOnline.Desktop.Views");
            // ViewModelLocator.AddNamespaceMapping("TrainsOnline.Desktop.ViewModels.*", "TrainsOnline.Desktop.ViewModels");

            _container = new WinRTContainer();
            _container.AddPresentation();
            _container.AddApplication();
            _container.AddInfrastructure();
        }

        protected override object GetInstance(Type service, string key)
        {
            return _container.GetInstance(service, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return _container.GetAllInstances(service);
        }

        protected override void BuildUp(object instance)
        {
            _container.BuildUp(instance);
        }

        private ActivationService CreateActivationService()
        {
            return new ActivationService(_container, typeof(ViewModels.HomeViewModel), new Lazy<UIElement>(CreateShell));
        }

        private UIElement CreateShell()
        {
            Views.ShellPage shellPage = new Views.ShellPage();
            _container.RegisterInstance(typeof(IConnectedAnimationService), nameof(IConnectedAnimationService), new ConnectedAnimationService(shellPage.GetFrame()));
            return shellPage;
        }
    }
}
