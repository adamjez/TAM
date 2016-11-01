using System;
using System.Numerics;
using Windows.Graphics.Display;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Graphics.Canvas.UI;
using Microsoft.Graphics.Canvas.UI.Xaml;
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
        }


        private void ImageTest_OnImageOpened(object sender, RoutedEventArgs e)
        {
            //var _compositor = ElementCompositionPreview.GetElementVisual(this).Compositor;

            //// Create an effect description 
            //GaussianBlurEffect blurEffect = new GaussianBlurEffect()
            //{
            //    Name = "Blur",
            //    BlurAmount = 10.0f,
            //    BorderMode = EffectBorderMode.Hard,
            //    Optimization = EffectOptimization.Balanced
            //};

            //blurEffect.Source = new CompositionEffectSourceParameter("source");

            //CompositionEffectFactory blurEffectFactory = _compositor.CreateEffectFactory(blurEffect);
            //CompositionEffectBrush blurBrush = blurEffectFactory.CreateBrush();
            //// Create a BackdropBrush and bind it to the EffectSourceParameter “source” 
            //CompositionBackdropBrush backdropBrush = _compositor.CreateBackdropBrush();
            //blurBrush.SetSourceParameter("source", backdropBrush);

            //// The SpriteVisual to apply the blur BackdropBrush to 
            //// This will cause everything behind this SpriteVisual to be blurred 
            //SpriteVisual blurSprite = _compositor.CreateSpriteVisual();
            //blurSprite.Brush = blurBrush;

            //// Set blurSprite as a child visual of a XAML element 
            //ElementCompositionPreview.SetElementChildVisual(TestGrid, blurSprite);
        }

        private void CanvasControl_OnDraw(CanvasControl sender, CanvasDrawEventArgs args)
        {
            if (imageLoaded)
            {
                using (var session = args.DrawingSession)
                {
                    session.Units = CanvasUnits.Pixels;

                    double displayScaling = DisplayInformation.GetForCurrentView().LogicalDpi / 96.0;

                    double pixelWidth = sender.ActualWidth * displayScaling;

                    var scalefactor = pixelWidth / image.Size.Width;

                    scaleEffect.Source = this.image;
                    scaleEffect.Scale = new Vector2()
                    {
                        X = (float)scalefactor,
                        Y = (float)scalefactor
                    };

                    blurEffect.Source = scaleEffect;
                    blurEffect.BlurAmount = 10.0f;

                    session.DrawImage(blurEffect, 0.0f, 0.0f);
                }
            }
        }

        private async void CanvasControl_OnCreateResources(CanvasControl sender, CanvasCreateResourcesEventArgs args)
        {
            if (!string.IsNullOrWhiteSpace(ImageUrl))
            {
                scaleEffect = new ScaleEffect();
                blurEffect = new GaussianBlurEffect();

                image = await CanvasBitmap.LoadAsync(sender.Device,
                  new Uri(ImageUrl));

                imageLoaded = true;

                sender.Invalidate();
            }
        }

        private bool imageLoaded;
        private ScaleEffect scaleEffect;
        private GaussianBlurEffect blurEffect;
        private string ImageUrl = "http://api.play.cz/static/radio_logo/playuk40.png";
        private CanvasBitmap image;
    }
}
