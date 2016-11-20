using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using OnRadio.App.ViewModels;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Microsoft.Toolkit.Uwp;
using OnRadio.App.Views;

namespace OnRadio.App.Common
{
    public class MvvmPage : Page
    {
        public MvvmPage()
        {
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
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

        public async Task ShowNetworkErroDialog()
        {
            MessageDialog dialog = new MessageDialog("Bohužel není dostupné potřebné internetové připojení." +
                                                     " Zkontrojte nastavení sítě a spusťte aplikaci znovu.", "Nastala chyba");
            dialog.Commands.Add(new UICommand("Rozumím", CloseApplication));
            await DispatcherHelper.ExecuteOnUIThreadAsync(() => dialog.ShowAsync());
        }
        public async Task ShowErroDialog(string message, string title)
        {
            MessageDialog dialog = new MessageDialog(message, title);
            dialog.Commands.Add(new UICommand("Rozumím", NavigateToRadioList));
            await DispatcherHelper.ExecuteOnUIThreadAsync(() => dialog.ShowAsync());
        }

        private void CloseApplication(IUICommand command)
        {
            CoreApplication.Exit();
        }

        private void NavigateToRadioList(IUICommand command)
        {
            Frame.Navigate(typeof(RadioList));
            Frame.BackStack.Clear();
        }
    }
}