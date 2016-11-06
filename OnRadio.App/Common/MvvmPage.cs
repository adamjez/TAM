using System;
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
            var viewmodel = DataContext as LoadingViewModelBase;
            viewmodel?.Initialize(e.Parameter);

            base.OnNavigatedTo(e);
        }
    }
}