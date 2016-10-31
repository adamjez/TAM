using OnRadio.App.Common;
using Bezysoftware.Navigation.Lookup;
using OnRadio.App.ViewModels;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace OnRadio.App.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    [AssociatedViewModel(typeof(PlayerViewModel))]
    public sealed partial class Player : MvvmPage
    {
        public Player()
        {
            this.InitializeComponent();
        }
    }
}