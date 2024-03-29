﻿namespace TrainsOnline.Desktop.ViewModels
{
    using System;
    using System.Linq;
    using Caliburn.Micro;
    using TrainsOnline.Desktop.Helpers;
    using TrainsOnline.Desktop.Views;
    using Windows.System;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Input;
    using Windows.UI.Xaml.Navigation;
    using WinUI = Microsoft.UI.Xaml.Controls;

    public class ShellViewModel : Screen
    {
        private readonly KeyboardAccelerator _altLeftKeyboardAccelerator = BuildKeyboardAccelerator(VirtualKey.Left, VirtualKeyModifiers.Menu);
        private readonly KeyboardAccelerator _backKeyboardAccelerator = BuildKeyboardAccelerator(VirtualKey.GoBack);

        private readonly WinRTContainer _container;
        private static INavigationService _navigationService;
        private WinUI.NavigationView _navigationView;
        private bool _isBackEnabled;
        private WinUI.NavigationViewItem _selected;

        public ShellViewModel(WinRTContainer container)
        {
            _container = container;
        }

        public bool IsBackEnabled
        {
            get => _isBackEnabled;
            set => Set(ref _isBackEnabled, value);
        }

        public WinUI.NavigationViewItem Selected
        {
            get => _selected;
            set => Set(ref _selected, value);
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();
            IShellView view = GetView() as IShellView;

            _navigationService = view?.CreateNavigationService(_container);
            _navigationView = view?.GetNavigationView();

            if (_navigationService != null)
            {
                _navigationService.NavigationFailed += (sender, e) =>
                {
                    throw e.Exception;
                };
                _navigationService.Navigated += NavigationService_Navigated;
                _navigationView.BackRequested += OnBackRequested;
            }
        }

        protected override void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            if (GetView() is UIElement page)
            {
                page.KeyboardAccelerators.Add(_altLeftKeyboardAccelerator);
                page.KeyboardAccelerators.Add(_backKeyboardAccelerator);
            }
        }

        private void OnItemInvoked(WinUI.NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked)
            {
                _navigationService.Navigate(typeof(SettingsPage));
                return;
            }

            WinUI.NavigationViewItem item = _navigationView.MenuItems.OfType<WinUI.NavigationViewItem>()
                                                                     .First(menuItem => (string)menuItem.Content == (string)args.InvokedItem);
            Type pageType = item.GetValue(NavHelper.NavigateToProperty) as Type;
            Type viewModelType = ViewModelLocator.LocateTypeForViewType(pageType, false);
            _navigationService.NavigateToViewModel(viewModelType);
        }

        private void OnBackRequested(WinUI.NavigationView sender, WinUI.NavigationViewBackRequestedEventArgs args)
        {
            _navigationService.GoBack();
        }

        private void NavigationService_Navigated(object sender, NavigationEventArgs e)
        {
            IsBackEnabled = _navigationService.CanGoBack;
            if (e.SourcePageType == typeof(SettingsPage))
            {
                Selected = _navigationView.SettingsItem as WinUI.NavigationViewItem;
                return;
            }

            Selected = _navigationView.MenuItems
                            .OfType<WinUI.NavigationViewItem>()
                            .FirstOrDefault(menuItem => IsMenuItemForPageType(menuItem, e.SourcePageType));
        }

        private bool IsMenuItemForPageType(WinUI.NavigationViewItem menuItem, Type sourcePageType)
        {
            Type sourceViewModelType = ViewModelLocator.LocateTypeForViewType(sourcePageType, false);
            Type pageType = menuItem.GetValue(NavHelper.NavigateToProperty) as Type;
            Type viewModelType = ViewModelLocator.LocateTypeForViewType(pageType, false);
            return viewModelType == sourceViewModelType;
        }

        private static KeyboardAccelerator BuildKeyboardAccelerator(VirtualKey key, VirtualKeyModifiers? modifiers = null)
        {
            KeyboardAccelerator keyboardAccelerator = new KeyboardAccelerator() { Key = key };
            if (modifiers.HasValue)
            {
                keyboardAccelerator.Modifiers = modifiers.Value;
            }

            keyboardAccelerator.Invoked += OnKeyboardAcceleratorInvoked;
            return keyboardAccelerator;
        }

        private static void OnKeyboardAcceleratorInvoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            if (_navigationService.CanGoBack)
            {
                _navigationService.GoBack();
                args.Handled = true;
            }
        }
    }
}
