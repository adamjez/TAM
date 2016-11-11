using System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using OnRadio.App.ViewModels;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace OnRadio.App.Common
{
    public class MvvmPage : Page
    {
        public MvvmPage()
        {
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var viewmodel = DataContext as LoadingViewModelBase;
            viewmodel?.StartLoadData();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame != null && rootFrame.CanGoBack)
            {
                // Show UI in title bar if opted-in and in-app backstack is not empty.
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                    AppViewBackButtonVisibility.Visible;
            }
            else
            {
                // Remove the UI from the title bar if in-app back stack is empty.
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                    AppViewBackButtonVisibility.Collapsed;
            }

            var viewmodel = DataContext as LoadingViewModelBase;
            viewmodel?.Initialize(e.Parameter);


            base.OnNavigatedTo(e);
        }
    }
}