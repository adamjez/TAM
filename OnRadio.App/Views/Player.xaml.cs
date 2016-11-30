using System;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using GalaSoft.MvvmLight.Messaging;
using OnRadio.App.Common;
using OnRadio.App.Messages;

namespace OnRadio.App.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Player : MvvmPage
    {
        public Player()
        {
            this.InitializeComponent();
            Unloaded += Unload;
            Messenger.Default.Register<OpenDialogMessage>(this, OpenDialog);
            Messenger.Default.Register<CloseDialogMessage>(this, CloseDialog);
        }

        private void Unload(object sender, RoutedEventArgs e)
        {
            Messenger.Default.Unregister<OpenDialogMessage>(this, OpenDialog);
            Messenger.Default.Unregister<CloseDialogMessage>(this, CloseDialog);
        }

        public async void OpenDialog(OpenDialogMessage message)
        {
            if (message.DialogName == "TimerDialog")
            {
                await TimerDialog.ShowAsync();
            }
        }

        public void CloseDialog(CloseDialogMessage message)
        {
            if (message.DialogName == "TimerDialog")
            {
                TimerDialog.Hide();
            }
        }

        private void ListViewBase_OnItemClick(object sender, ItemClickEventArgs e)
        {
            QualityFlyout.Hide();
        }

        private async void MediaButtonClick(object sender, RoutedEventArgs e)
        {
            var url = (string)((Button)sender).Tag;
            var uri = new Uri(url);

            await Launcher.LaunchUriAsync(uri);
        }
    }
}
