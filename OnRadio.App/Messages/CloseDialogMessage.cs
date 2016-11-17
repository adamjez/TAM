using Windows.UI.Xaml;
using GalaSoft.MvvmLight.Messaging;

namespace OnRadio.App.Messages
{
    public class CloseDialogMessage : MessageBase
    {
        public string DialogName { get; set; }

        public CloseDialogMessage(object sender, string dialogName)
            : base(sender)
        {
            Sender = sender;
            DialogName = dialogName;
        }
    }
}