﻿using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Media.Animation;
using OnRadio.App.Common;

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

            var compositor = ElementCompositionPreview.GetElementVisual(this).Compositor;
        }
    }
}
