using System;
using System.Numerics;
using Windows.Foundation;
using Windows.Graphics.Imaging;
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Media.Imaging;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Toolkit.Uwp.UI.Controls;
using OnRadio.App.Common;
using OnRadio.App.Helpers;
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
            Messenger.Default.Register<OpenDialogMessage>(this, OpenDialog);
            Messenger.Default.Register<CloseDialogMessage>(this, CloseDialog);
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
    }
}
