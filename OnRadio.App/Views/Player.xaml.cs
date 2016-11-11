using System;
using System.Numerics;
using Windows.Foundation;
using Windows.Graphics.Imaging;
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Media.Imaging;
using Microsoft.Toolkit.Uwp.UI.Controls;
using OnRadio.App.Common;
using OnRadio.App.Helpers;

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
        }
    }
}
