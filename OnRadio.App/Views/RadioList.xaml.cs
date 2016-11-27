using Windows.UI.Xaml;
using OnRadio.App.Common;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace OnRadio.App.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class RadioList : MvvmPage
    {
        public RadioList()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Enabled;
        }

        private void UIElement_OnHolding(object sender, HoldingRoutedEventArgs e)
        {
            //FrameworkElement senderElement = sender as FrameworkElement;
            //// If you need the clicked element:
            //// Item whichOne = senderElement.DataContext as Item;
            //FlyoutBase flyoutBase = FlyoutBase.GetAttachedFlyout(senderElement);
            //flyoutBase.ShowAt(senderElement);
        }
    }
}