using OnRadio.App.ViewModels;
using Windows.UI.Xaml.Controls;

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
    }
}