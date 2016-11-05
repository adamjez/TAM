using System.Numerics;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Microsoft.Graphics.Canvas.Text;
using Microsoft.Graphics.Canvas.UI;
using Microsoft.Graphics.Canvas.UI.Xaml;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace OnRadio.App.Controls
{
    public sealed partial class MiniPlayer : UserControl
    {
        public MiniPlayer()
        {
            this.InitializeComponent();
        }

        private void CanvasControl_OnDraw(CanvasControl sender, CanvasDrawEventArgs args)
        {
            var textFormat = new CanvasTextFormat() {FontSize = 62};
            args.DrawingSession.DrawText("Win2D", new Vector2(0, 0), Color.FromArgb(0xff, 0x42, 0x81, 0xA4), textFormat);
            args.DrawingSession.DrawCircle(new Vector2(50, 50), 50, Color.FromArgb(255, 255, 0, 0));
        }

        private void CanvasControl_OnCreateResources(CanvasControl sender, CanvasCreateResourcesEventArgs args)
        {
        }
    }
}
